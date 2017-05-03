Imports ServerDownClassLibrary

Class MainWindow
    Implements IView
#Region "Declaration"
    Private control As ServerDownClientControl
    Private viewList As ViewList
    Public ReadOnly Property StatusLogList() As ListBox
        Get
            Return listBoxLog
        End Get
    End Property
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
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        viewList = New ViewList(listBoxLog)
        control = New ServerDownClientControl(Me)
    End Sub

    Private Sub Window_Closing(sender As Object, e As ComponentModel.CancelEventArgs)
        control.StopTimer()
    End Sub
#End Region
End Class
