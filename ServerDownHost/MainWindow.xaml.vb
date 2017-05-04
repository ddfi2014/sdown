Imports ServerDownClassLibrary

Partial Class MainWindow
    Implements IView
#Region "Declaration"
    Private control As ServerDownHostControl = Nothing
    Private viewList As IViewList = Nothing
#End Region
#Region "Initialization"
    ''' <summary>
    ''' Registers the view at a control object.
    ''' </summary>
    Private Sub SetControl()
        Try
            viewList = New ViewList(listBoxLog)
            control = ServerDownHostControl.GetInstance()
            ServerDownHostControl.AddView(Me)
        Catch ex As Exception
            Console.WriteLine("ERROR: Couldn't set control to ServerDownHostControl.")
            Close()
        End Try
    End Sub
#End Region
#Region "Implementation"
    Public Sub AddLogItem(item As String) Implements IView.AddLogItem
        listBoxLog.Items.Add(item)
    End Sub

    Public Sub ClearList() Implements IView.ClearList
        listBoxLog.Items.Clear()
    End Sub

    Public Sub ShowErrorMessage(message As String, Optional errorlevel As EErrorLevel = EErrorLevel.None) Implements IView.ShowErrorMessage
        Select Case errorlevel
            Case EErrorLevel.None

            Case EErrorLevel.Hint
                MessageBox.Show(message, errorlevel.ToString(), MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK)
            Case EErrorLevel.Warning
                MessageBox.Show(message, errorlevel.ToString(), MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK)
            Case EErrorLevel.CriticalError
                MessageBox.Show(message, errorlevel.ToString(), MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK)
            Case Else

        End Select
    End Sub

    Public Sub Dispatch(priority As Threading.DispatcherPriority, method As [Delegate]) Implements IView.Dispatch
        Dispatcher.BeginInvoke(priority, method)
    End Sub

    Public Function GetStatusMessage() As String Implements IView.GetStatusMessage
        Return statusBarItemMessage.Content
    End Function

    Public Sub SetStatusMessage(message As String) Implements IView.SetStatusMessage
        statusBarItemMessage.Content = message
    End Sub

    Public Function GetStatusLogList() As IViewList Implements IView.GetStatusLogList
        Return viewList
    End Function
#End Region
#Region "Events"
    'Private Sub buttonStart_Click(sender As Object, e As RoutedEventArgs) Handles buttonStart.Click
    '    control.InitializeHost()
    'End Sub

    'Private Sub buttonStop_Click(sender As Object, e As RoutedEventArgs) Handles buttonStop.Click
    '    control.CloseHost()
    'End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        SetControl()
    End Sub
#End Region
End Class