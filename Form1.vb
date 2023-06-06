Imports System.IO

Public Class Form1
    Dim DataPath As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\Surface"

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Çıkış butonu
        Application.Exit()
    End Sub
    Private Sub DataBase_Check()
        'Veri yolunu doğrulama

        If Not Directory.Exists(DataPath) Then
            Directory.CreateDirectory(DataPath)
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DataBase_Check()
    End Sub
End Class
