Imports System.Net.Http
Imports System.Text
Imports System.Text.Json

Public Class Eventos
    Public Property eventoId As Long
    Public Property nombreEvento As String
    Public Property descripcion As String
    Public Property fecha As Date?
    Public Property ubicacion As String
    Public Property Secciones As String
End Class

Public Class Evento
    Public Property Id As Integer = 0
    Public Property Eventos As List(Of Eventos)

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        CargarEventos()
        DataContext = Me

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private txtNombreEventoTB As Object
    Private txtDescripcionTB As Object
    Private dpFechaTb As Object
    Private txtUbicacionTB As Object

    Public Sub AgregarSeccion(ubicaccion As String, precio As Double)
        ' Event.Secciones.Add(New Secciones(ubicacion:=txtUbicacion, precio:=precio))
    End Sub

    Private Sub Resgistrar(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Async Sub Resgistrar_Click(sender As Object, e As RoutedEventArgs)

        Dim ev As New Eventos()
        ev.nombreEvento = txtNombreEventoTB
        ev.fecha = dpFechaTb.SelectedDate
        ev.descripcion = txtDescripcionTB
        ev.ubicacion = txtUbicacionTB

        Using clientEvento As New HttpClient()
            clientEvento.BaseAddress = New Uri("http://localhost:8090/")

            Dim EventoJson As String = JsonSerializer.Serialize(ev)
            Dim content = New StringContent(EventoJson, Encoding.UTF8, "application/json")


            Dim response = Await clientEvento.PostAsync("/eventos/save/evento", content)

            If response.IsSuccessStatusCode Then
                MessageBox.Show("El evento se a guardado correctamente.")

                Dim boletosGuardado As HttpResponseMessage = Await clientEvento.GetAsync("/eventos/list/evento")

                If boletosGuardado.IsSuccessStatusCode Then
                    Dim responseContent As String = Await boletosGuardado.Content.ReadAsStringAsync()
                    Dim boletosEnApi As List(Of Eventos) = JsonSerializer.Deserialize(Of List(Of Eventos))(responseContent)

                    Dim boletosWindow As New Boleto(boletosEnApi.LastOrDefault())
                    boletosWindow.txtNombreEvento.Text = txtNombreEvento.Text
                    boletosWindow.Show()
                    Close()
                End If
            Else
                MessageBox.Show("No se pudo guardar el evento. Inténtalo nuevamente.")
            End If
        End Using

    End Sub

    Private Sub CargarEventos()

        Using httClient As New HttpClient()
            Dim response As HttpResponseMessage = httClient.GetAsync("http://localhost:8090/eventos/list/evento").Result

            If response.IsSuccessStatusCode Then
                Dim responseContent As String = response.Content.ReadAsStringAsync().Result
                Eventos = JsonSerializer.Deserialize(Of List(Of Eventos))(responseContent)
                listEventos.ItemsSource = Eventos
            Else
                MessageBox.Show("No pudo cargar los eventos.")
            End If
        End Using
    End Sub
End Class
