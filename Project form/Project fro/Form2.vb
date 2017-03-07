Public Class Form2
    'Start of defining lists and global variables
    Dim HelmList As New List(Of Form1.helm)
    Dim CrewList As New List(Of Form1.crew)
    Dim DutiesList As New List(Of Form1.DutiesSocial)
    Dim index As Integer = 0
    Dim crewIndex As Integer = 0
    Dim SailorFilename As String = "\\newtoy\pupils$\2010\10KNOCHA\A-level IT\Compuing project\Sailor data.txt"
    Dim admin As Boolean = Form1.sendAdmin
    'End of defining lists and global variables

    'Start of Form2 events
    Private Sub Form2_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        loadSailorData()
        DisplayHelmData()
        displayCrewData()

        If admin = True Then
            createEditButton()
        End If

        lblCount.Text = HelmList.Count
        lblIndex.Text = index + 1

    End Sub

    'Start of edit list
    Sub createEditButton()
        Dim mybutton As New Button
        mybutton.Left = 636
        mybutton.Top = 131
        mybutton.Text = "Edit"
        mybutton.Width = 75
        mybutton.Height = 23

        AddHandler mybutton.Click, AddressOf EditButton

        Me.Controls.Add(mybutton)

    End Sub

    Sub EditButton()
        HelmList.Item(index).getData(txtHelmSname.Text, txtHelmFname.Text, dtpHelmDoB.Value, txtHelmClub.Text, Val(txtHelmPhone.Text), txtHelmEmail.Text, txtHelmAddress1.Text & ", " & txtHelmAddress2.Text & ", " & txtHelmAddress3.Text, Val(txtSailnumber.Text), cboClass.Text, chkHelmQualifying.CheckState, chkSingle.CheckState)

        If HelmList.Item(index).returnSingle = False Then
            CrewList.Item(crewIndex).getData(txtCrewSname.Text, txtCrewFname.Text, dtpCrewDoB.Value, txtCrewClub.Text, Val(txtCrewPhone.Text), txtCrewEmail.Text, txtCrewAddress1.Text & ", " & txtCrewAddress2.Text & ", " & txtCrewAddress3.Text, Val(txtSailnumber.Text), cboClass.Text, chkCrewQualifying.CheckState, chkSpinnaker.CheckState)
            crewIndex += 1
        End If

        DutiesList.Item(index).getData(nudMon.Value, nudTue.Value, nudWed.Value, nudThu.Value, nudFri.Value, nudBlacktie.Value, nudAdultbop.Value, nudFinaldisco.Value, nudFridaygames.Value, Val(txtSailnumber.Text))

        lblCount.Text = HelmList.Count
        lblIndex.Text = index + 1

    End Sub
    'End of edit list

    'Start of arrow keys
    Private Sub btnNext_Click(sender As System.Object, e As System.EventArgs) Handles btnNext.Click
        Dim SailnumberArray(CrewList.Count - 1) As Integer
        For i = 0 To SailnumberArray.Length - 1
            SailnumberArray(i) = CrewList.Item(i).returnSailNumber
        Next

        If index < HelmList.Count - 1 Then 'You can increment if you are not outputing blank 
            index += 1
            DisplayHelmData()
            If HelmList.Item(index).returnSingle = False Then
                crewIndex = integerLinearSearch(SailnumberArray, HelmList.Item(index).returnSailnumber)
                displayCrewData()
            Else
                clearCrewData()
            End If

        End If

        lblIndex.Text = index + 1

    End Sub

    Private Sub btnPrev_Click(sender As System.Object, e As System.EventArgs) Handles btnPrev.Click
        Dim SailnumberArray(CrewList.Count - 1) As Integer
        For i = 0 To SailnumberArray.Length - 1
            SailnumberArray(i) = CrewList.Item(i).returnSailnumber
        Next

        If index > 0 Then
            index -= 1
            DisplayHelmData()
            If HelmList.Item(index).returnSingle = False Then
                crewIndex = integerLinearSearch(SailnumberArray, HelmList.Item(index).returnSailnumber)
                displayCrewData()
            Else
                clearCrewData()
            End If

        End If

        lblIndex.Text = index + 1

    End Sub
    'End of arrow keys

    'Start of display data
    Sub DisplayHelmData()
        HelmList.Item(index).editForm2Data()
    End Sub

    Sub displayCrewData()
        If HelmList.Item(index).returnSingle = False Then
            CrewList.Item(crewIndex).editForm2Data()
        Else
            clearCrewData()
        End If
    End Sub
    'End of display data

    Sub clearCrewData()
        txtCrewSname.Clear()
        txtCrewFname.Clear()
        dtpCrewDoB.Text = Date.Now
        txtCrewClub.Clear()
        txtCrewPhone.Clear()
        txtCrewEmail.Clear()
        txtCrewAddress1.Clear()
        txtCrewAddress2.Clear()
        txtCrewAddress3.Clear()
        chkCrewQualifying.CheckState = False
        chkSpinnaker.CheckState = False
    End Sub 'Clear all the textboxes

    'Start of save/load
    Sub saveSailorData()
        FileOpen(1, SailorFilename, OpenMode.Output)
        WriteLine(1, HelmList.Count - 1) 'this is so when loading i can know the helm data ends and so start loading to CrewList
        For i = 0 To HelmList.Count - 1
            HelmList(i).saveSailorData()
        Next
        WriteLine(1, CrewList.Count - 1) 'this is so when loading i can know when the crew data ends so I can start loading to dutiesList
        For i = 0 To CrewList.Count - 1
            CrewList(i).saveSailorData()
        Next
        WriteLine(1, DutiesList.Count - 1)
        For i = 0 To DutiesList.Count - 1
            DutiesList(i).saveDutiesData()
        Next
        MsgBox("Saved")
        FileClose(1)
    End Sub

    Sub loadSailorData()
        Dim endOfHelm As Integer
        Dim endOfCrew As Integer
        Dim endOfDuties As Integer

        HelmList.Clear()
        CrewList.Clear()
        DutiesList.Clear()

        FileOpen(1, SailorFilename, OpenMode.Input)

        Input(1, endOfHelm)
        For i = 0 To endOfHelm 'this will take all the helm values and input them to helmList
            HelmList.Add(New Form1.helm())
            HelmList(i).loadSailorData()
        Next
        Input(1, endOfCrew)
        For i = 0 To endOfCrew 'this will take all the crew values and input them to crewList
            CrewList.Add(New Form1.crew())
            CrewList(i).loadSailorData()
        Next
        Input(1, endOfDuties)
        For i = 0 To endOfDuties
            DutiesList.Add(New Form1.DutiesSocial())
            DutiesList(i).loadDutiesData()
        Next
        FileClose(1)
    End Sub
    'End of save/load

    'Start of sort/search buttons
    Private Sub btnGoTo_Click(sender As System.Object, e As System.EventArgs) Handles btnGoTo.Click
        Dim SailnumberArray(CrewList.Count - 1) As Integer
        For i = 0 To SailnumberArray.Length - 1
            SailnumberArray(i) = CrewList.Item(i).returnSailnumber
        Next

        If Val(txtGoTo.Text) < HelmList.Count + 1 And Val(txtGoTo.Text) > 0 Then
            index = Val(txtGoTo.Text) - 1
            DisplayHelmData()
            If HelmList.Item(index).returnSingle = False Then
                crewIndex = integerLinearSearch(SailnumberArray, HelmList.Item(index).returnSailnumber)
                displayCrewData()
            Else
                clearCrewData()
            End If
            lblIndex.Text = index + 1
        Else
            MsgBox("that is not one of the items")
        End If

    End Sub

    Private Sub btnSearchName_Click(sender As System.Object, e As System.EventArgs) Handles btnSearchName.Click
        Dim searchTerm As String = txtSearchName.Text
        Dim nameArray(HelmList.Count - 1) As String
        Dim foundlocation As Integer

        For i = 0 To nameArray.Length - 1
            nameArray(i) = HelmList.Item(i).returnSurname
        Next
        foundlocation = stringLinearSearch(nameArray, searchTerm)

        If foundlocation <> -1 Then
            index = foundlocation
            Dim SailnumberArray(CrewList.Count - 1) As Integer
            For i = 0 To SailnumberArray.Length - 1
                SailnumberArray(i) = CrewList.Item(i).returnSailnumber
            Next
            DisplayHelmData()
            If HelmList.Item(index).returnSingle = False Then
                crewIndex = integerLinearSearch(SailnumberArray, HelmList.Item(index).returnSailnumber)
                displayCrewData()
            Else
                clearCrewData()
            End If
            lblIndex.Text = index + 1
        End If
    End Sub

    Private Sub btnSort_Click(sender As System.Object, e As System.EventArgs) Handles btnSort.Click
        Dim SailnumberArray(CrewList.Count - 1) As Integer
        For i = 0 To SailnumberArray.Length - 1
            SailnumberArray(i) = CrewList.Item(i).returnSailnumber
        Next

        If cboSort.Text = "Surname" Then
            Dim swapped As Boolean = False
            Dim temp As Form1.helm
            Do
                swapped = False
                For i = 1 To HelmList.Count - 1
                    If HelmList.Item(i - 1).returnSurname > HelmList.Item(i).returnSurname Then
                        temp = HelmList.Item(i)
                        HelmList.Item(i) = HelmList.Item(i - 1)
                        HelmList.Item(i - 1) = temp
                        swapped = True
                    End If
                Next
            Loop While swapped = True
            DisplayHelmData()
            If HelmList.Item(index).returnSingle = False Then
                crewIndex = integerLinearSearch(SailnumberArray, HelmList.Item(index).returnSailnumber)
                displayCrewData()
            Else
                clearCrewData()
            End If
        ElseIf cboSort.Text = "Class" Then
            Dim boatType() As String = {"420", "Laser", "Senior Feva", "Junior Feva", "Senior Topper", "Junior Topper", "Senior Mirror", "Junior Mirror", "Senior Optimist", "Junior Optimist"}
            Dim pointer As Integer = 0 'points to the end of the boat class which boatType is currently searching for
            Dim temp As Form1.helm
            For i = 0 To boatType.Length - 1
                For j = 0 To HelmList.Count - 1
                    If HelmList.Item(j).returnBoatClass = boatType(i) Then
                        temp = HelmList.Item(j)
                        HelmList.Item(j) = HelmList.Item(pointer)
                        HelmList.Item(pointer) = temp
                        pointer += 1
                    End If
                Next
            Next
        End If

        DisplayHelmData()
        If HelmList.Item(index).returnSingle = False Then
            crewIndex = integerLinearSearch(SailnumberArray, HelmList.Item(index).returnSailnumber)
            displayCrewData()
        Else
            clearCrewData()
        End If
    End Sub
    'End of sort/search buttons

    'Start of sort/search functions
    Function integerLinearSearch(ByVal array() As Integer, ByVal target As Integer) As Integer
        Dim found As Boolean = False
        For i = 0 To array.Length - 1
            If array(i) = target Then
                found = True
                Return i
            End If
        Next

        Return -1

    End Function

    Function stringLinearSearch(ByVal array() As String, ByVal target As String) As Integer
        Dim found As Boolean = False
        For i = 0 To array.Length - 1
            If array(i) = target Then
                found = True
                Return i
            End If
        Next

        Return -1

    End Function
    'End of sort/search funtions

    Private Sub btnDone_Click(sender As System.Object, e As System.EventArgs) Handles btnDone.Click
        If admin = True Then
            saveSailorData()
        End If
        Me.Close()
    End Sub 'Save and close Form2
    'End of Form2 events

End Class