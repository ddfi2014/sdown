Imports System.Threading.Tasks
Imports System.Timers

Public Class ServerDownClientControl

#Region "Declarations"
    Private proxy As New GetLogClient()
    Private clientView As MainWindow
    Public serverLog As String()
    Private timer As Timers.Timer
    Private Const interval As Double = 1000 * 15
    'Printersettings
#End Region

#Region "Instance"
    Public Sub New()
        timer = New Timers.Timer(interval)
        AddHandler timer.Elapsed, AddressOf OnTimedEvent
        timer.AutoReset = True
        timer.Start()
    End Sub

    Public Sub New(clientView As MainWindow)
        Me.New()
        Me.clientView = clientView
        InitializeWindow()
    End Sub
#End Region

#Region "Functionality"
    Public Sub InitializeWindow()
        'clientView.listBoxLog.Items.Clear()
        clientView.listBoxLog.ItemsSource = serverLog
        clientView.statusBarItemMessage.Content = ""
    End Sub

    Public Sub Print(Optional ByVal isQuickPrint As Boolean = False)
        If isQuickPrint Then
            PrintNow()
        Else
            PrintComplex()
        End If
    End Sub

    Private Sub PrintNow()
        Throw New NotImplementedException("before complex: default; after complex: Save Printersettings")
    End Sub

    Private Sub PrintComplex()
        Dim printDialog As New PrintDialog()
        printDialog.PageRangeSelection = PageRangeSelection.AllPages
        printDialog.UserPageRangeEnabled = True

        Dim print As Boolean? = printDialog.ShowDialog()
        If print Then

        End If
    End Sub

    Public Sub GetTextLog()
        Try
            serverLog = proxy.GetServerLog()
            clientView.Dispatcher.BeginInvoke(Threading.DispatcherPriority.Background, New Action(Sub() UpdateList()))
        Catch ex As Exception
            Console.WriteLine(ex.StackTrace)
        End Try
    End Sub

    Public Sub StopTimer()
        timer.Stop()
    End Sub
#End Region

#Region "Events"
    Private Sub OnTimedEvent(sender As Object, e As ElapsedEventArgs)
        GetTextLog()
    End Sub
#End Region

    Public Sub UpdateList()
        clientView.listBoxLog.ItemsSource = serverLog
    End Sub
End Class
