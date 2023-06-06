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
        'Başlangıçta

        DataBase_Check()
        Timer1.Start()
    End Sub
    Private Sub AddCategories()
        TreeView1.Nodes.Clear()
        For Each isim As String In My.Computer.FileSystem.GetDirectories(DataPath)
            Dim result As String = Path.GetFileName(isim)
            TreeView1.Nodes.Add(result)
        Next
    End Sub
    Public Sub Treeview_Adder(ByVal directoryValue As String, ByVal parentNode As TreeNode)
        'Treeview'e belirlenen klasörün altındaki klasörleri ekler
        Try

            Dim directoryArray As String() = Directory.GetDirectories(directoryValue)

            If directoryArray.Length <> 0 Then
                Dim currentDirectory As String

                For Each currentDirectory In directoryArray
                    Dim myNode As TreeNode = New TreeNode(currentDirectory)
                    parentNode.Nodes.Add(myNode)
                Next

            End If
        Catch unauthorized As UnauthorizedAccessException
            parentNode.Nodes.Add("Access Denied*")
        End Try
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        If Not TreeView1.Nodes.Count = My.Computer.FileSystem.GetDirectories(DataPath).Count Then
            AddCategories()
        End If

    End Sub
End Class
