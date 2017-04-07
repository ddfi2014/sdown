Imports System.ServiceModel
Imports System.Timers

Public Class ServerDownHostControl

#Region "Declarations"
    Private sh As ServiceHost
    Private Shared instance As ServerDownHostControl = Nothing
    Private Shared listViews As List(Of MainWindow) = Nothing
    Private logIsOn As Boolean = False
    Private logText As New List(Of String)
    Private timer As Timer = Nothing
    Private Const interval As Double = 1000 * 1 * 5
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
    ''' <summary>
    ''' Adds a view to the list of controlled views.
    ''' </summary>
    ''' <param name="view"></param>
    Public Shared Sub AddView(ByRef view As MainWindow)
        listViews.Add(view)
        GetInstance().InitializeWindow()
    End Sub

    ''' <summary>
    ''' Initializes all views.
    ''' </summary>
    Public Sub InitializeWindow()
        For Each view In listViews
            view.listBoxLog.Items.Clear()
            view.statusBarItemMessage.Content = ""
        Next
    End Sub

    ''' <summary>
    ''' Updates the listBox's position to show the most recent entry.
    ''' </summary>
    ''' <param name="listbox"></param>
    Public Sub UpdateListBoxPosition(listbox As ListBox)
        If listbox IsNot Nothing Then
            Dim _border = TryCast(VisualTreeHelper.GetChild(listbox, 0), Border)
            Dim _scrollViewer = TryCast(VisualTreeHelper.GetChild(_border, 0), ScrollViewer)
            _scrollViewer.ScrollToBottom()
        End If
    End Sub

    ''' <summary>
    ''' Sets the host's state in the statusbars of all views.
    ''' </summary>
    ''' <param name="message"></param>
    Private Sub SetHostState(message As String)
        For Each view In listViews
            view.statusBarItemMessage.Content = message
        Next
    End Sub

    ''' <summary>
    ''' Starts the hosting.
    ''' </summary>
    Public Sub InitializeHost()
#Region "Fallback"
        'Throw New NotImplementedException("InitializeHost()")
        'Host with WCF
        ''Using sh As New ServiceHost(GetType(GetLog))
        ''    sh.Open()
        ''    Console.WriteLine("Service bereit...")
        ''    Console.ReadLine()
        ''End Using
#End Region
        sh = New ServiceHost(GetType(GetLog))
        sh.Open()
        SetHostState("Service ready.")
        Console.WriteLine("Service ready.")
        Console.ReadLine()
    End Sub

#Region "DBGTest"
    <Obsolete()>
    Public Sub TestHost()
        InitializeHost()
        SetLogState(runOnce:=True)
        CloseHost()
    End Sub
#End Region

    ''' <summary>
    ''' Stops the hosting.
    ''' </summary>
    Public Sub CloseHost()
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
        UpdateListBox()
    End Sub

    Private Sub UpdateListBox()
        For Each view In listViews
            view.listBoxLog.Items.Clear()
            For Each item In logText
                view.listBoxLog.Items.Add(item)
            Next
        Next
    End Sub
#End Region

    <Obsolete()>
    Public Function GetLogText() As String()
        Return logText.ToArray()
    End Function
#End Region

#Region "Logging"
    ''' <summary>
    ''' Turns the Logging on when it is off and vice versa.
    ''' </summary>
    ''' <param name="runOnce"></param>
    ''' <param name="isConsole"></param>
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

    ''' <summary>
    ''' Starts the logging process.
    ''' </summary>
    ''' <param name="runOnce"></param>
    ''' <param name="isConsole"></param>
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
                StartTimer()
            Else
                Dim result As MessageBoxResult = MessageBox.Show("Do you want to start a test?", "Logging Test", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Cancel)
                If result.Equals(MessageBoxResult.Yes) Then
                    TestServers()
                End If
            End If
#End Region
        End If
    End Sub

    ''' <summary>
    ''' Tests, if the specified servers are running and updates the views.
    ''' </summary>
    Public Sub TestServers()
        logText = ServerTest.GetInstance().GetServerState()
        For Each view In listViews
            view.Dispatcher.BeginInvoke(Threading.DispatcherPriority.Background, New Action(Sub() UpdateListBox(view)))
        Next
    End Sub

    ''' <summary>
    ''' Stops the logging process.
    ''' </summary>
    Public Sub EndLog()
        'Throw New NotImplementedException("EndLog()")
        'Stop logging servers
        StopTimer()
    End Sub
#End Region

#Region "Timer"
    Private Sub StartTimer()
        timer = New Timer(interval)
        AddHandler timer.Elapsed, AddressOf OnTimedEvent
        timer.AutoReset = True
        timer.Start()
    End Sub

    Private Sub StopTimer()
        timer.Stop()
    End Sub

    Private Sub OnTimedEvent(sender As Object, e As ElapsedEventArgs)
        TestServers()
    End Sub
#End Region

#Region "Delegates"
    ''' <summary>
    ''' Updates the view's listBox.
    ''' </summary>
    ''' <param name="view"></param>
    Public Sub UpdateListBox(ByRef view As MainWindow)
        'view.listBoxLog.ItemsSource = logText
        For Each item In logText
            view.listBoxLog.Items.Add(item)
        Next
        UpdateListBoxPosition(view.listBoxLog)
        view.statusBarItemMessage.Content = "New Report: " & DateTime.Now.ToLongTimeString()
    End Sub
#End Region

End Class