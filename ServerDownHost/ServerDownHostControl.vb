Imports System.ServiceModel

Public Class ServerDownHostControl

#Region "Declarations"
    Private Shared instance As ServerDownHostControl = Nothing
    Private logIsOn As Boolean = False
    Private logText As String()
#End Region

#Region "Singleton-Pattern"
    Private Sub New()

    End Sub
    Public Shared Function GetInstance() As ServerDownHostControl
        If instance Is Nothing Then
            instance = New ServerDownHostControl()
        End If
        Return instance
    End Function
#End Region

    Public Sub InitializeHost()
        'Throw New NotImplementedException("InitializeHost()")
        'Host with WCF
        'Using sh As New ServiceHost(GetType(GetLog))
        '    sh.Open()
        '    Console.WriteLine("Service bereit...")
        '    Console.ReadLine()
        'End Using
        'Dim sh As New ServiceHost(GetType(GetLog))
        'sh.Open()
        'Console.WriteLine("Service bereit...")
        'Console.ReadLine()
    End Sub

    Public Sub CloseHost()
        Throw New NotImplementedException("CloseHost()")
        'Host with WCF
    End Sub

    Public Function GetLogText() As String()
        Return logText
    End Function

#Region "Logging"
    Public Sub SetLogState(Optional ByVal runOnce As Boolean = False, Optional ByVal isConsole As Boolean = False)
        If logIsOn.Equals(False) Then
            Try
                StartLog(runOnce, isConsole)
                logIsOn = True
            Catch ex As Exception
                Console.WriteLine(ex.StackTrace)
            End Try
        Else
            Try
                EndLog()
                logIsOn = False
            Catch ex As Exception
                Console.WriteLine(ex.StackTrace)
            End Try
        End If
    End Sub

    Public Sub StartLog(Optional ByVal runOnce As Boolean = False, Optional ByVal isConsole As Boolean = False)
        If isConsole Then
#Region "isConsole = True"
            If Not runOnce Then
                Throw New NotImplementedException("runOnce = False; isConsole = True")
            Else
                Dim reply As String
                Console.WriteLine("Do you want to start a test? Y/N")
                reply = Console.ReadLine()
                If reply.ToLower().First().Equals("y") Or reply.ToLower().First().Equals("j") Then
                    TestServers()
                ElseIf reply.ToLower().First().Equals("n") Then
                    Console.WriteLine("Test aborted.")
                    Console.ReadLine()
                Else
                    Console.WriteLine("Invalid reply.")
                End If
            End If
#End Region
        Else
#Region "isConsole = False"
            If Not runOnce Then
                Throw New NotImplementedException("runOnce = False; isConsole = False")
            Else
                Dim result As MessageBoxResult = MessageBox.Show("Do you want to start a test?", "Logging Test", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Cancel)
                If result.Equals(MessageBoxResult.Yes) Then
                    TestServers()
                End If
            End If
#End Region
        End If
    End Sub

    Public Sub TestServers()
        Throw New NotImplementedException("TestServers()")
        'Testing the servers's availability (separate class?)
    End Sub

    Public Sub EndLog()
        Throw New NotImplementedException("EndLog()")
        'Stop logging servers
    End Sub
#End Region

End Class
