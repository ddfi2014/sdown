Imports ServerDownClassLibrary

Class MainWindow
    Implements IView
#Region "Declaration"
    Private control As ServerDownClientControl
    Public Property StatusMessage() As String
        Get
            Return statusBarItemMessage.Content
        End Get
        Set(value As String)
            statusBarItemMessage.Content = value
        End Set
    End Property
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
#End Region
#Region "Events"
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        control = New ServerDownClientControl(Me)
    End Sub

    Private Sub SaveLog_Click(sender As Object, e As RoutedEventArgs) Handles buttonSaveLog.Click, menuItemOptionsSave.Click
        control.Save()
    End Sub

    Private Sub SaveLogAs_Click(sender As Object, e As RoutedEventArgs) Handles menuItemOptionsSaveAs.Click
        control.SaveAs()
    End Sub

    Private Sub buttonTest_Click(sender As Object, e As RoutedEventArgs) Handles buttonTest.Click
        control.GetTextLog()
    End Sub

    Private Sub Window_Closing(sender As Object, e As ComponentModel.CancelEventArgs)
        control.StopTimer()
    End Sub
#End Region
End Class
