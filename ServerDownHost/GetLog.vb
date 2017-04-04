' HINWEIS: Mit dem Befehl "Umbenennen" im Kontextmenü können Sie den Klassennamen "GetLog" sowohl im Code als auch in der Konfigurationsdatei ändern.
Public Class GetLog
    Implements IGetLog

    Public Function GetServerLog() As String() Implements IGetLog.GetServerLog
        Dim log As String() = ServerDownHostControl.GetInstance().GetLogText()
        Return log
    End Function

End Class
