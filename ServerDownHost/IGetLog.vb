Imports System.ServiceModel

' HINWEIS: Mit dem Befehl "Umbenennen" im Kontextmenü können Sie den Schnittstellennamen "IGetLog" sowohl im Code als auch in der Konfigurationsdatei ändern.
<ServiceContract()>
Public Interface IGetLog

    <OperationContract()>
    Function GetServerLog() As String()

End Interface
