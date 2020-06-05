using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDKHrobot;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace _12.NetWork {
    class Program {
        static int device_id = -1;
        static List<string> recv_val = new List<string>();
        static bool is_received = true;
        public static ManualResetEvent allDone = new ManualResetEvent( false );
        private static HRobot.CallBackFun callback = new HRobot.CallBackFun( EventFun );

        static void Main( string[] args ) {
            device_id = HRobot.open_connection( "127.0.0.1", 1, callback );
            Thread.Sleep( 1000 );
            StringBuilder sdk_version = new StringBuilder();
            HRobot.get_hrsdk_version( sdk_version );
            Console.WriteLine( "SDK version: " + sdk_version );
            if ( device_id >= 0 ) {
                Console.WriteLine( "connect successful." );
                HRobot.set_motor_state( device_id, 1 );
                Thread notify = new Thread( run_notify );
                notify.IsBackground = true;
                notify.Start( device_id );

                NetWorkExample_Client( device_id ); // HRSS is socket client
                Thread.Sleep( 2000 );
                Console.WriteLine();
                NetWorkExample_Server( device_id ); // HRSS is socket server

                Thread.Sleep( 3000 );
                Console.WriteLine( " \n Please press enter key to end." );

                Console.ReadLine();
                HRobot.disconnect( device_id );
            } else {
                Console.WriteLine( "connect failure." );
            }
        }

        // Callback notify setting
        public static void EventFun( UInt16 cmd, UInt16 rlt, ref UInt16 Msg, int len ) {
            String info_p = "";
            unsafe {
                fixed ( UInt16* p = &Msg ) {
                    for ( int i = 0; i < len; i++ ) {
                        info_p += ( char )p[i];
                    }
                }
            }
            if ( cmd == 0 && rlt == 4034 && len > 1 ) {
                // split received  messages
                string[] words = info_p.Split( new string[] { "&&&" }, System.StringSplitOptions.RemoveEmptyEntries );
                // splitting parameter values
                for ( int i = 0; i < words.Length; i++ ) {
                    recv_val.Clear();
                    if ( words[i].IndexOf( "Read" ) == -1 ) {
                        continue;
                    }
                    int data_begin = words[i].IndexOf( '{' ) + 1;
                    int data_len = words[i].IndexOf( '}' ) - words[i].IndexOf( '{' ) - 1;
                    string str2 = words[i].Substring( data_begin, data_len );
                    string[] split_val = str2.Split( new string[] { "," }, System.StringSplitOptions.RemoveEmptyEntries );
                    for ( int j = 0; j < split_val.Length; j++ ) {
                        recv_val.Add( split_val[j] );
                    }
                    is_received = true;
                    Console.WriteLine( "[NETWORK NOTIFY]: " + words[i] );
                }
            }
        }

        // If you want to get the network information, you does have this function.
        // The received parameter is set to the value of the counter.
        public static void run_notify( object device_id ) {
            int id = ( int )device_id;
            Console.WriteLine( "Strat Notify. " );
            double[] pos = new double[6];
            HRobot.get_current_position( id, pos );
            while ( true ) {
                HRobot.network_get_state( id );
                if ( is_received ) {
                    for ( int i = 1; i <= recv_val.Count; i++ ) {
                        if ( Int32.TryParse( recv_val[i - 1], out int tmp ) ) {
                            HRobot.set_counter( id, i, tmp );
                        }
                    }
                    is_received = false;
                }
                Thread.Sleep( 200 );
            }
        }

        // HRSS send the message to another local socket.
        public static void send_msg( int device_id ) {
            int x = 0;
            Console.WriteLine( "The received data is set as counter value." );
            while ( true ) {
                string str;
                string str_num1 = x.ToString();
                string str_num2 = ( x + 1 ).ToString();
                string str_num3 = ( x + 2 ).ToString();
                str = str_num1 + "," + str_num2 + "," + str_num3 + "%%%";
                StringBuilder send_msg = new StringBuilder( str );
                HRobot.network_send_msg( device_id, send_msg );
                Thread.Sleep( 1000 );
                if ( x == 10 ) {
                    HRobot.network_disconnect( device_id );
                    break;
                }
                x++;
            }
        }

        // HRSS is socket client
        public static void NetWorkExample_Client( int device_id ) {
            int client_type = 1; // socket client
            StringBuilder ip = new StringBuilder( "127.0.0.1" );
            int port = 5000;
            int bracket = 0;
            int separator = 0;
            bool is_format = false;
            int show_msg = -1;

            // create local socket server
            Thread create_server = new Thread( socketServer );
            create_server.IsBackground = true;
            create_server.Start();
            Thread.Sleep( 2000 );

            // network setting.
            HRobot.set_network_config( device_id, client_type, ip, port, bracket, separator, is_format );
            HRobot.network_connect( device_id );
            HRobot.get_network_show_msg( device_id, ref show_msg );
            if ( show_msg != 1 ) {
                HRobot.set_network_show_msg( device_id, 1 );
            }
            Console.WriteLine( "HRSS is socket client." );

            send_msg( device_id );
        }

        // HRSS is socket server
        public static void NetWorkExample_Server( int device_id ) {
            int server_type = 0; // socket server
            StringBuilder ip = new StringBuilder( "127.0.0.1" );
            int port = 5123;
            int bracket = 0;
            int separator = 0;
            bool is_format = false;
            int show_msg = -1;

            // network setting
            HRobot.set_network_config( device_id, server_type, ip, port, bracket, separator, is_format );
            HRobot.network_connect( device_id );
            HRobot.get_network_show_msg( device_id, ref show_msg );
            if ( show_msg != 1 ) {
                HRobot.set_network_show_msg( device_id, 1 );
            }
            Console.WriteLine( "HRSS is socket server." );
            Thread.Sleep( 2000 );

            // create local socket client
            Thread create_client = new Thread( socketClient );
            create_client.IsBackground = true;
            create_client.Start();

            send_msg( device_id );
        }

        // This is a local socket server.
        // If you want to connect the other HIWIN robot, you can delete this socket function.
        public static void socketServer() {
            IPEndPoint localEndPoint = new IPEndPoint( IPAddress.Parse( "127.0.0.1" ), 5000 );
            // Create a TCP/IP socket.
            Socket listener = new Socket( AddressFamily.InterNetwork,
                                          SocketType.Stream, ProtocolType.Tcp );

            // Bind the socket to the local endpoint and listen for incoming connections.
            listener.Bind( localEndPoint );
            listener.Listen( 20 );
            Socket connection = null;
            connection = listener.Accept();
            Console.WriteLine( "Create a local socket server." );
            while ( true ) {
                byte[] idrec = new byte[50];
                int length = connection.Receive( idrec );
                if ( length == 0 ) {
                    break;
                }
                string recv_data = Encoding.UTF8.GetString( idrec, 0, length );
                connection.Send( Encoding.UTF8.GetBytes( recv_data ) );
                Thread.Sleep( 50 );
            }
            connection.Shutdown( SocketShutdown.Both );
            connection.Close();
            listener.Close();
            Console.WriteLine( "Local socket server is closed." );
        }

        // This is a local socket client.
        // If you want to connect the other HIWIN robot, you can delete this socket function.
        public static void socketClient() {
            IPEndPoint remoteIP = new IPEndPoint( IPAddress.Parse( "127.0.0.1" ), 5123 );
            Socket client = new Socket( AddressFamily.InterNetwork,
                                        SocketType.Stream, ProtocolType.Tcp );
            try {
                client.Connect( remoteIP );
                Console.WriteLine( "Create a local socket client.  " + client );
                while ( true ) {
                    byte[] idrec = new byte[50];
                    int length = client.Receive( idrec );
                    if ( length == 0 ) {
                        break;
                    }
                    string recv_data = Encoding.UTF8.GetString( idrec, 0, length );
                    client.Send( Encoding.UTF8.GetBytes( recv_data ) );
                    Thread.Sleep( 50 );
                }
                client.Shutdown( SocketShutdown.Both );
                client.Close();
                Console.WriteLine( "Local socket client is closed." );
            } catch( SocketException  e ) {
                Console.Write( "Fail to connect server. " );
                Console.Write( e.ToString() );
            }
        }

    }
}