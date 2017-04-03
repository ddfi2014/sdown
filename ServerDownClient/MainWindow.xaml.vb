Class MainWindow

#Region "Declarations"
    Dim control As New ServerDownClientControl()
#End Region

#Region "Functionality"
    Private Sub PrintLog(Optional ByVal isQuickPrint As Boolean = False)
        If isQuickPrint Then
            control.Print(isQuickPrint)
        Else
            control.Print()
        End If
    End Sub
#End Region

#Region "Events"
    Private Sub buttonPrintLog_Click(sender As Object, e As RoutedEventArgs) Handles buttonPrintLog.Click
        PrintLog(isQuickPrint:=True)
    End Sub

    Private Sub menuItemOptionsPrint_Click(sender As Object, e As RoutedEventArgs) Handles menuItemOptionsPrint.Click
        PrintLog(isQuickPrint:=True)
    End Sub

    Private Sub menuItemOptionsPrintDialog_Click(sender As Object, e As RoutedEventArgs) Handles menuItemOptionsPrintDialog.Click
        PrintLog()
    End Sub
#End Region

End Class
