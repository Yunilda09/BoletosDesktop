Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json

Partial Public Class Evento
    Public Property Eventos As List(Of Evento)
    Public Sub New()
        InitializeComponent()
        DataContext = Me
        'CargarEvento()
    End Sub

    <JsonIgnore>
    Public Property DataContext As Object


    Private txtNombreEvento As Object
    Private txtTipoEvento As Object
    Private dFecha As Object
    Private txtUbicacion As Object
    Private txtDescripcion As Object
    Private txtHora As Object

    Private Async Sub Registrar_Click(sender As Object, e As RoutedEventArgs)
        Dim evento As New Evento()

        evento.nombreEvento = nombreEvento
        evento.tipoEvento = descripcion
        'evento.fecha = dFecha.SelectedDate
        evento.ubicacion = ubicacion
        ' evento.descripcion = txtDescripcionTB.Text

        Using client As New HttpClient()

            client.BaseAddress = New Uri("http://localhost:8090/")
            Dim settings As New JsonSerializerSettings()
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore

            Dim jsonEvento As String = JsonConvert.SerializeObject(evento, settings)


            'Dim jsonEvento As String = JsonConvert.SerializeObject(evento)
            Dim content = New StringContent(jsonEvento, Encoding.UTF8, "application/json")


            Dim response = Await client.PostAsync("/eventos/evento/save", content)

            If response.IsSuccessStatusCode Then
                MessageBox.Show("El evento ha sido guardado correctamente.")
                Dim boletosWindow As New Boleto()
                boletosWindow.txtNombreEventoTB = txtNombreEvento
                boletosWindow.Evento = evento
                boletosWindow.Show()
                Close()
            Else
                MessageBox.Show("No se pudo guardar el evento. Inténtalo nuevamente.")
            End If
        End Using
    End Sub


    Private Sub LimpiarCampos()
        txtNombreEvento.Text = ""
        txtTipoEvento.Text = ""
        dFecha.SelectedDate = Date.Now
        txtUbicacion.Text = ""
        txtDescripcion.Text = ""
    End Sub



    Private Sub CargarEvento()
        Using httpClient As New HttpClient()
            Dim response As HttpResponseMessage = httpClient.GetAsync("http://localhost:8090/eventos/list/evento").Result

            If response.IsSuccessStatusCode Then
                Dim responseContent As String = response.Content.ReadAsStringAsync().Result
                Eventos = JsonConvert.DeserializeObject(Of List(Of Evento))(responseContent)
                listEvento.ItemsSource = Eventos
            Else
                MessageBox.Show("No se pudo obtener los eventos.")
            End If
        End Using
    End Sub

    Private Sub EditarEvento_Click(sender As Object, e As RoutedEventArgs)

    End Sub


End Class

Public Class Evento
    Public Property eventoId As Long
    Public Property nombreEvento As String
    Public Property tipoEvento As String
    Public Property fecha As Date
    Public Property ubicacion As String
    Public Property descripcion As String
End Class

