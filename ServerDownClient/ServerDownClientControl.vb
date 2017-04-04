Public Class ServerDownClientControl

#Region "Declarations"
    Private proxy As New GetLogClient()
    'Printersettings
#End Region

#Region "Instance"
    Public Sub New()

    End Sub
#End Region

#Region "Functionality"
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
        Throw New NotImplementedException("printDialog, etc.")
    End Sub
#End Region

End Class
