Imports System.ServiceModel

Public Class ServerDownHostControl

#Region "Declarations"
    Private sh As ServiceHost
    Private Shared instance As ServerDownHostControl = Nothing
    Private Shared listViews As List(Of MainWindow) = Nothing
    Private logIsOn As Boolean = False
    Private logText As New List(Of String)
#End Region

#Region "Singleton-Pattern"
    Private Sub New()

    End Sub
    Public Shared Function GetInstance() As ServerDownHostControl
        If instance Is Nothing Then
            instance = New ServerDownHostControl()
            listViews = New List(Of MainWindow)()
        End If
        Return instance
    End Function
#End Region

#Region "Control"
    Public Shared Sub AddView(ByRef view As MainWindow)
        listViews.Add(view)
        GetInstance().InitializeWindow()
    End Sub

    Public Sub InitializeWindow()
        For Each view In listViews
            view.listBoxLog.Items.Clear()
            view.statusBarItemMessage.Content = ""
        Next
    End Sub

    Private Sub SetHostState(message As String)
        For Each view In listViews
            view.statusBarItemMessage.Content = message
        Next
    End Sub

    Public Sub InitializeHost()
        'Throw New NotImplementedException("InitializeHost()")
        'Host with WCF
        ''Using sh As New ServiceHost(GetType(GetLog))
        ''    sh.Open()
        ''    Console.WriteLine("Service bereit...")
        ''    Console.ReadLine()
        ''End Using
        sh = New ServiceHost(GetType(GetLog))
        sh.Open()
        SetHostState("Service ready.")
        Console.WriteLine("Service ready.")
        Console.ReadLine()
    End Sub

    Public Sub TestHost()
        InitializeHost()
        SetLogState(runOnce:=True)
        CloseHost()
    End Sub

    Public Sub CloseHost()
        'Throw New NotImplementedException("CloseHost()")
        'Host with WCF
        Try
            sh.Close()
            SetHostState("Service stopped.")
        Catch ex As Exception
            SetHostState("Error: Couldn't stop Service.")
        End Try
    End Sub

#Region "LogTest"
    Public Sub CreateDummyLog()
        logText.Add("This is a test.")
        For Each view In listViews
            view.listBoxLog.Items.Clear()
            For Each item In logText
                view.listBoxLog.Items.Add(item)
            Next
        Next
    End Sub
#End Region

    Public Function GetLogText() As String()
        Return logText.ToArray()
    End Function
#End Region

#Region "Logging"
    Public Sub SetLogState(Optional ByVal runOnce As Boolean = False, Optional ByVal isConsole As Boolean = False)
        If logIsOn.Equals(False) Then
            Try
                StartLog(runOnce, isConsole)
                logIsOn = True
            Catch ex As Exception
                Console.WriteLine(ex.StackTrace)
                logIsOn = False
            End Try
        Else
            Try
                EndLog()
                logIsOn = False
            Catch ex As Exception
                Console.WriteLine(ex.StackTrace)
                logIsOn = True
            End Try
        End If
    End Sub

    Public Sub StartLog(Optional ByVal runOnce As Boolean = False, Optional ByVal isConsole As Boolean = False)
        If isConsole Then
#Region "isConsole = True"
            If Not runOnce Then
                Throw New NotImplementedException("runOnce = False; isConsole = True")
            Else
                Throw New NotImplementedException("runOnce = True; isConsole = True")
                'Dim reply As String
                'Console.WriteLine("Do you want to start a test? Y/N")
                'reply = Console.ReadLine()
                'If reply.ToLower().First().Equals("y") Or reply.ToLower().First().Equals("j") Then
                '    TestServers()
                'ElseIf reply.ToLower().First().Equals("n") Then
                '    Console.WriteLine("Test aborted.")
                '    Console.ReadKey()
                '    Throw New Exception("Test aborted.")
                'Else
                '    Console.WriteLine("Invalid reply.")
                '    Console.ReadKey()
                '    Throw New Exception("Invalid reply.")
                'End If
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
