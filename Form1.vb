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
                    If result2.Contains("#" + result + "#") Then
                        Dim once As String = result2.Remove(0, result2.IndexOf("#") + 1)
                        TreeView1.Nodes(index).Nodes.Add(once.Remove(0, once.IndexOf("#") + 1))
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

        If TreeView1.Focused = False Then
            ToolStripMenuItem2.Enabled = False
        Else
            ToolStripMenuItem2.Enabled = True
        End If

        If TreeView1.SelectedNode IsNot Nothing And TreeView1.Focused = True Then
            ToolStripMenuItem8.Enabled = True
        ElseIf ListView1.FocusedItem IsNot Nothing Then
            ToolStripMenuItem8.Enabled = True
        ElseIf ListView2.FocusedItem IsNot Nothing Then
            ToolStripMenuItem8.Enabled = True
        Else
            ToolStripMenuItem8.Enabled = False
        End If

        TreeView1.ExpandAll()
    End Sub

    Private Sub TreeView1_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView1.AfterSelect
        SubCategory_Selected = False
        Category_Selected = True
        If TreeView1.SelectedNode.Parent IsNot Nothing Then
            SubCategory_Selected = True
        End If

    End Sub
    Private Sub Reloads(type As String)
        'Reloads of listview 1 and 2
        Try
            Dim path As String
            If TreeView1.SelectedNode.Parent Is Nothing Then
                path = DataPath + TreeView1.SelectedNode.Text
            Else
                Dim name As String = DataPath + TreeView1.SelectedNode.Parent.Text & "\" + "#" + TreeView1.SelectedNode.Parent.Text + "#"
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

                    If TreeView1.SelectedNode.Parent Is Nothing Then
                        Dim sayi As Integer = TreeView1.SelectedNode.Text.Length + 2
                        If sonuc.Length < sayi Then
                            ListView2.Items.Add(sonuc, 0)
                        Else
                            Dim devam = sonuc.Remove(sayi, sonuc.Length - sayi)
                            If Not devam.Contains("#" + TreeView1.SelectedNode.Text + "#") Then
                                ListView2.Items.Add(sonuc, 0)
                            End If
                        End If

                    Else
                        ListView2.Items.Add(sonuc, 0)
                    End If
                Next
            End If

        Catch ex As Exception
            TreeView1.Nodes.Clear()
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub ListView1_DoubleClick(sender As Object, e As EventArgs) Handles ListView1.DoubleClick
        Try
            If Not ListView1.FocusedItem.Index < 0 Then
                If TreeView1.SelectedNode.Parent Is Nothing Then
                    Process.Start(DataPath + TreeView1.SelectedNode.Text + "\" + ListView1.FocusedItem.Text)
                Else
                    Process.Start(DataPath + TreeView1.SelectedNode.Parent.Text + "\#" + TreeView1.SelectedNode.Parent.Text + "#" + TreeView1.SelectedNode.Text + "\" + ListView1.FocusedItem.Text)
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub ListView2_DoubleClick(sender As Object, e As EventArgs) Handles ListView2.DoubleClick
        Try
            If Not ListView2.FocusedItem.Index < 0 Then
                If TreeView1.SelectedNode.Parent Is Nothing Then
                    Process.Start(DataPath + TreeView1.SelectedNode.Text + "\" + ListView2.FocusedItem.Text)
                Else
                    Process.Start(DataPath + TreeView1.SelectedNode.Parent.Text + "\#" + TreeView1.SelectedNode.Parent.Text + "#" + TreeView1.SelectedNode.Text + "\" + ListView2.FocusedItem.Text)
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub TreeView1_BeforeCollapse(sender As Object, e As TreeViewCancelEventArgs) Handles TreeView1.BeforeCollapse
        e.Cancel = True
    End Sub

    Private Sub ToolStripMenuItem7_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem7.Click
        QuickRefresh()
    End Sub

    Private Sub CategoryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CategoryToolStripMenuItem.Click
        Dim name As String = InputBox("Name of your new Category: ")
        If Not name = vbNullString Then
            Try
                Directory.CreateDirectory(DataPath + name)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical)
            End Try
        End If
    End Sub

    Private Sub SubCategoryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SubCategoryToolStripMenuItem.Click
        Dim name As String = InputBox("Name of your new SubCategory: ")
        If Not name = vbNullString Then
            Try
                Directory.CreateDirectory(DataPath + TreeView1.SelectedNode.Text + "\#" + TreeView1.SelectedNode.Text + "#" + name)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical)
            End Try
        End If
    End Sub

    Private Sub FileToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles FileToolStripMenuItem1.Click
        Dim f As New OpenFileDialog
        f.Title = "Select your file to move: "
        f.Multiselect = True
        f.ShowDialog()
        Dim path As String
        If f.FileNames IsNot Nothing Then
            If TreeView1.SelectedNode.Parent IsNot Nothing Then
                path = DataPath + TreeView1.SelectedNode.Parent.Text + "\#" + TreeView1.SelectedNode.Parent.Text + "#" + TreeView1.SelectedNode.Text + "\"
            Else
                path = DataPath + TreeView1.SelectedNode.Text + "\"
            End If
            For Each file As String In f.FileNames
                Try
                    Dim safe = file.Split("\").Last
                    My.Computer.FileSystem.MoveFile(file, path + safe)
                Catch ex As Exception
                    MsgBox(ex.Message, MsgBoxStyle.Critical)
                End Try
            Next
        End If

    End Sub

    Private Sub FolderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FolderToolStripMenuItem.Click
        Dim f As New FolderBrowserDialog
        f.Description = "Select your folder to move: "
        f.ShowDialog()
        Dim path As String
        If Not f.SelectedPath = vbNullString Then
            If TreeView1.SelectedNode.Parent IsNot Nothing Then
                path = DataPath + TreeView1.SelectedNode.Parent.Text + "\#" + TreeView1.SelectedNode.Parent.Text + "#" + TreeView1.SelectedNode.Text + "\"
            Else
                path = DataPath + TreeView1.SelectedNode.Text + "\"
            End If
            Dim file As String = f.SelectedPath
            Try
                Dim safe = file.Split("\").Last
                My.Computer.FileSystem.MoveDirectory(file, path + safe)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical)
            End Try
        End If
    End Sub

    Private Sub ToolStripMenuItem8_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem8.Click
        Dim focused As String
        If TreeView1.Focused = True Then
            focused = "TreeView1"
        ElseIf ListView1.Focused = True Then
            focused = "ListView1"
        ElseIf ListView2.Focused = True Then
            focused = "ListView2"
        Else
            Exit Sub
        End If

        Dim path As String
        Try
            If TreeView1.SelectedNode.Parent IsNot Nothing Then
                path = DataPath + TreeView1.SelectedNode.Parent.Text + "\#" + TreeView1.SelectedNode.Parent.Text + "#" + TreeView1.SelectedNode.Text + "\"
            Else
                path = DataPath + TreeView1.SelectedNode.Text + "\"
            End If
        Catch ex As Exception
            MsgBox("Please select a category to use this function!", MsgBoxStyle.Critical)
            Exit Sub
        End Try


        Try
            If focused = "ListView1" Then
                Dim msg As MsgBoxResult
                msg = MsgBox("Do you want to delete selected item?", MsgBoxStyle.OkCancel)
                If msg = MsgBoxResult.Ok Then
                    File.Delete(path + ListView1.FocusedItem.Text)
                Else
                    MsgBox("The operation was canceled..")
                End If

            ElseIf focused = "ListView2" Then
                Dim msg As MsgBoxResult
                msg = MsgBox("Do you want to delete selected item?", MsgBoxStyle.OkCancel)
                If msg = MsgBoxResult.Ok Then
                    Directory.Delete(path + ListView2.FocusedItem.Text, SearchOption.AllDirectories)
                Else
                    MsgBox("The operation was canceled..")
                End If

            ElseIf focused = "TreeView1" Then
                Dim msg As MsgBoxResult
                msg = MsgBox("Do you want to delete selected item?", MsgBoxStyle.OkCancel)
                If msg = MsgBoxResult.Ok Then
                    Directory.Delete(path, SearchOption.AllDirectories)
                Else
                    MsgBox("The operation was canceled..")
                End If

            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
        QuickRefresh()
    End Sub
    Private Sub QuickRefresh()
        AddCategories()
        ListView1.Items.Clear()
        ListView2.Items.Clear()
    End Sub

    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem2.Click
        Dim path As String
        Try
            If TreeView1.SelectedNode.Parent IsNot Nothing Then
                path = DataPath + TreeView1.SelectedNode.Parent.Text + "\#" + TreeView1.SelectedNode.Parent.Text + "#" + TreeView1.SelectedNode.Text + "\"
            Else
                path = DataPath + TreeView1.SelectedNode.Text + "\"
            End If
            Process.Start(path)
        Catch ex As Exception
            MsgBox("Please select a category to use this function!", MsgBoxStyle.Critical)
            Exit Sub
        End Try
    End Sub

    Private Sub GoAppDataLocationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GoAppDataLocationToolStripMenuItem.Click
        Try
            Process.Start(DataPath)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub
End Class
