Imports System.IO

Public Class Form1
    Dim DataPath As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\Surface\"

    Dim Category_Selected As Boolean
    Dim SubCategory_Selected

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

        Dim index As Integer = -1

        TreeView1.Nodes.Clear()

        For Each isim As String In My.Computer.FileSystem.GetDirectories(DataPath)
            Dim result As String = Path.GetFileName(isim)
            TreeView1.Nodes.Add(result)
            index += 1
            If Directory.GetDirectories(isim).Count > 0 Then

                For Each name As String In Directory.GetDirectories(isim)
                    Dim result2 As String = Path.GetFileName(name)
                    If result2.Contains("(" + result + ")") Then
                        Dim once As String = result2.Remove(0, result2.IndexOf("(") + 1)
                        TreeView1.Nodes(index).Nodes.Add(once.Remove(0, once.IndexOf(")") + 1))
                        'Treeview_Adder(isim, TreeView1.Nodes(index))
                    End If
                Next

            End If
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
                    parentNode.Nodes.Add(myNode.ToString.Remove(0, myNode.ToString.LastIndexOf("\") + 1))
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

        If Category_Selected = True Then
            Category_Selected = False
            Reloads("file")
            Reloads("folder")
        End If


    End Sub

    Private Sub TreeView1_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView1.AfterSelect
        SubCategory_Selected = False
        Category_Selected = True
        If TreeView1.SelectedNode.Parent IsNot Nothing Then
            SubCategory_Selected = True
        End If

    End Sub
    Private Function Reloads(type As String)
        'Reloads of listview 1 and 2
        Try
            Dim path As String
            If SubCategory_Selected = False Then
                path = DataPath + TreeView1.SelectedNode.Text
            Else
                Dim name As String = DataPath + TreeView1.SelectedNode.Parent.Text & "\" + "(" + TreeView1.SelectedNode.Parent.Text + ")"
                path = name & TreeView1.SelectedNode.Text
            End If

            If type = "file" Then
                imageList1.Images.Clear()
                ListView1.Items.Clear()
                ListView1.BeginUpdate()
                Dim di As New IO.DirectoryInfo(path)
                For Each fi As IO.FileInfo In di.GetFiles("*")
                    Dim icons As Icon = SystemIcons.WinLogo
                    Dim li As New ListViewItem(fi.Name, 1)
                    If Not (imageList1.Images.ContainsKey(fi.Name)) Then
                        icons = System.Drawing.Icon.ExtractAssociatedIcon(fi.FullName)
                        imageList1.Images.Add(fi.Name, icons)
                    End If
                    icons = Icon.ExtractAssociatedIcon(fi.FullName)
                    imageList1.Images.Add(icons)
                    ListView1.Items.Add(fi.Name, fi.Name)
                Next
                ListView1.EndUpdate()
            End If


            If type = "folder" Then
                ListView2.Items.Clear()
                For Each kats In My.Computer.FileSystem.GetDirectories(path)
                    Dim sonuc As String = kats.Split("\").Last
                    Dim sayi As Integer = TreeView1.SelectedNode.Text.Length + 2
                    If Not SubCategory_Selected = True Then
                        If sonuc.Length < sayi Then
                            ListView2.Items.Add(sonuc, 0)
                            Exit Function
                        End If
                        Dim devam = sonuc.Remove(sayi, sonuc.Length - sayi)
                        If Not devam = "(" + TreeView1.SelectedNode.Text + ")" Then
                            ListView2.Items.Add(sonuc, 0)
                        End If
                    End If
                    ListView2.Items.Add(sonuc, 0)
                Next
            End If

        Catch ex As Exception
            TreeView1.Nodes.Clear()
            MsgBox(ex.Message)
        End Try
    End Function
End Class
