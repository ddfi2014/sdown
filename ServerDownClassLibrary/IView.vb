Public Interface IView
    Sub AddLogItem(item As String)
    Sub ClearList()
    'Sub ToggleLogging()
    Sub ShowErrorMessage(message As String, Optional errorlevel As EErrorLevel = EErrorLevel.None)
End Interface
