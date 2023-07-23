Imports System.Text.Json
Imports System.Net.Http
Imports System.Text


Public Class Boletos

    Public Property eventoId As Long
    Public Property nombreEvento As String
    Public Property cantidadBoletos As Double
    Public Property asiento As String
    Public Property precio As Double

    Public Property evento As Eventos
End Class


Public Class Boleto
    Public Property Eventos As Eventos


    Public Sub New(ev As Eventos)
        Me.Eventos = ev

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub


    Private Async Sub GuardarBoleto_Click(sender As Object, e As RoutedEventArgs)

        If Eventos Is Nothing Then
            MessageBox.Show("No se ha asignado ningún evento.")
            Return
        End If

        Dim eventoId As Long = Eventos.eventoId
        Dim nombreEvento As String = txtNombreEvento.Text
        Dim cantidadBoletos As Double
        Dim precio As Double

        ' aqui se crea una instancia de boletos con los valores ingresados
        Dim b As New Boletos()
        b.nombreEvento = txtNombreEvento.Text
        b.evento = Eventos
        b.cantidadBoletos = If(Double.TryParse(txtCantidadBoletos.Text, cantidadBoletos), cantidadBoletos, 0.0)
        b.precio = If(Double.TryParse(txtPrecio.Text, precio), precio, 0.0)

        b.asiento = CType(cmbTipoAsiento.SelectedItem, ComboBoxItem).Content.ToString()


        Dim jsonBoletos As String = JsonSerializer.Serialize(b)
        Dim content As New StringContent(jsonBoletos, Encoding.UTF8, "application/json")

        Try

            Using httpClient As New HttpClient()
                Dim response As HttpResponseMessage = Await httpClient.PostAsync($"http://localhost:8090/boletos/save", content)

                If response.IsSuccessStatusCode Then
                    MessageBox.Show("Boletos guardados exitosamente")
                    Close()
                    Dim newWindow As New Evento()
                    newWindow.Show()
                Else
                    MessageBox.Show("Error al guardar los boletos")
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error en la solicitud HTTP: " & ex.Message)
        End Try
    End Sub
End Class