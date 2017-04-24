Imports System.ServiceModel
Imports System.Timers

Public Class ServerDownHostControl

#Region "Declarations"
    Private sh As ServiceHost
    Private Shared instance As ServerDownHostControl = Nothing
    Private Shared view As MainWindow = Nothing
    Private logIsOn As Boolean = False
    Private logText As New List(Of String)
    Private timer As Timer = Nothing
    Private Const interval As Double = 1000 * 1 * 5 'Critical Error after awhile with low intervals, i.e. 1 second.
    Private serverList As List(Of String) = Nothing
#End Region

#Region "Singleton-Pattern"
    Private Sub New()

    End Sub
    Public Shared Function GetInstance() As ServerDownHostControl
        If instance Is Nothing Then
            instance = New ServerDownHostControl()
            view = New MainWindow()
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
        'ServerDownHostControl.view.Add(view)
        ServerDownHostControl.view = view
        GetInstance().InitializeWindow()
    End Sub

    ''' <summary>
    ''' Initializes all views.
    ''' </summary>
    Public Sub InitializeWindow()
        'For Each view In view
        '    view.listBoxLog.Items.Clear()
        '    view.statusBarItemMessage.Content = ""
        'Next
        view.listBoxLog.Items.Clear()
        view.statusBarItemMessage.Content = ""
    End Sub

    ''' <summary>
    ''' Updates the ListBox's position to show the most recent entry.
    ''' </summary>
    ''' <param name="listBox"></param>
    Public Sub UpdateListBoxPosition(listBox As ListBox)
        If listBox IsNot Nothing Then
            Dim _border = TryCast(VisualTreeHelper.GetChild(listBox, 0), Border)
            Dim _scrollViewer = TryCast(VisualTreeHelper.GetChild(_border, 0), ScrollViewer)
            _scrollViewer.ScrollToBottom()
        End If
    End Sub

    ''' <summary>
    ''' Sets the host's state in the statusbars of all views.
    ''' </summary>
    ''' <param name="message"></param>
    Private Sub SetHostState(message As String)
        'For Each view In view
        '    view.statusBarItemMessage.Content = message
        'Next
        view.statusBarItemMessage.Content = message
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
        ToggleLogState(runOnce:=True)
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
        'For Each view In view
        '    view.listBoxLog.Items.Clear()
        '    For Each item In logText
        '        view.listBoxLog.Items.Add(item)
        '    Next
        'Next
        view.listBoxLog.Items.Clear()
        For Each item In logText
            view.listBoxLog.Items.Add(item)
        Next
    End Sub
#End Region

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
    <Obsolete()>
    Public Sub ToggleLogState(Optional ByVal runOnce As Boolean = False, Optional ByVal isConsole As Boolean = False)
        If logIsOn.Equals(False) Then
            Try
                StartLog(runOnce, isConsole)
                logIsOn = True
                view.statusBarItemMessage.Content = "Logging: Enabled."
            Catch ex As Exception
                Console.WriteLine(ex.StackTrace)
                logIsOn = False
            End Try
        Else
            Try
                EndLog()
                logIsOn = False
                view.statusBarItemMessage.Content = "Logging: Disabled."
            Catch ex As Exception
                Console.WriteLine(ex.StackTrace)
                logIsOn = True
            End Try
        End If
    End Sub

    Public Sub SetLogOn()
        If logIsOn = False Then
            Try
                StartLog()
                'serverList = ServerTest.GetServerList()
                logIsOn = True
                view.statusBarItemMessage.Content = "Logging: Enabled."
            Catch ex As Exception
                Console.WriteLine(ex.StackTrace)
                logIsOn = False
            End Try
        End If
    End Sub

    Public Sub SetLogOff()
        If logIsOn Then
            Try
                EndLog()
                logIsOn = False
                view.statusBarItemMessage.Content = "Logging: Disabled."
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
        logText = ServerTest.GetServerStateStrings()
        view.Dispatcher.BeginInvoke(Threading.DispatcherPriority.Background, New Action(Sub() UpdateListBox(view)))
    End Sub

    ''' <summary>
    ''' Stops the logging process.
    ''' </summary>
    Public Sub EndLog()
        StopTimer()
    End Sub
#End Region

#Region "Timer"
    ''' <summary>
    ''' Starts the timer, that triggers an event after a specified amount of time has passed.
    ''' </summary>
    Private Sub StartTimer()
        timer = New Timer(interval)
        AddHandler timer.Elapsed, AddressOf OnTimedEvent
        timer.AutoReset = True
        timer.Start()
    End Sub

    ''' <summary>
    ''' Stops the timer.
    ''' </summary>
    Private Sub StopTimer()
        timer.Stop()
    End Sub

    ''' <summary>
    ''' Calls the <c>TestServers()</c> method after being triggered by the <c>timer</c>.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub OnTimedEvent(sender As Object, e As ElapsedEventArgs)
        TestServers()
    End Sub
#End Region

#Region "Delegates"
    ''' <summary>
    ''' Updates the view's listBox.
    ''' </summary>
    ''' <param name="view"></param>
    Private Sub UpdateListBox(view As MainWindow)
        'view.listBoxLog.ItemsSource = logText
        'For Each item In logText
        '    view.listBoxLog.Items.Add(item)
        'Next
        logText.ForEach(Sub(item) view.listBoxLog.Items.Add(item))
        UpdateListBoxPosition(view.listBoxLog)
        view.statusBarItemMessage.Content = "New Report: " & DateTime.Now.ToLongTimeString()
    End Sub
#End Region

End Class