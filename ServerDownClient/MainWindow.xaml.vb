Class MainWindow

#Region "Declarations"
    Dim control As ServerDownClientControl = Nothing
#End Region

#Region "Events"
    Private Sub printLog_Click(sender As Object, e As RoutedEventArgs) Handles buttonPrintLog.Click, menuItemOptionsPrint.Click
        'custom printjob
        control.Print(isQuickPrint:=True)
    End Sub

    Private Sub menuItemOptionsPrintDialog_Click(sender As Object, e As RoutedEventArgs) Handles menuItemOptionsPrintDialog.Click
        'default printjob
        control.Print()
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        control = New ServerDownClientControl(Me)
    End Sub

    Private Sub buttonTest_Click(sender As Object, e As RoutedEventArgs) Handles buttonTest.Click
        control.GetTextLog()
    End Sub

    Private Sub Window_Closing(sender As Object, e As ComponentModel.CancelEventArgs)
        control.StopTimer()
    End Sub
#End Region

End Class
