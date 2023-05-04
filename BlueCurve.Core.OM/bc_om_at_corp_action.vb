
Imports BlueCurve.Core.CS
Imports System.IO
Imports System.Xml


<Serializable()> Public Class bc_om_corp_action
    Inherits bc_cs_soap_base_class

    Public ActionClassId As Long
    Public ActionEntityId As Long
    Public ActionEventId As Long
    Public ActionNewShares As Long
    Public ActionExDate As Date
    Public ActionPrice As Double
    Public ActionRatio As Integer
    Public ActionId As Long
    Public Submitted As Integer
    Public EntityDescription As String
    Public EventDescriprion As String
    Public EventType As String

    'Start Values
    Public StartShares As Double
    Public StartMcap As Double
    Public StartPrice As Double

    'Calculated values
    Public CalcNewShares As Long
    Public CalcCapIncreased As Long
    Public CalcStockPrice As Double
    Public CalcCashDividend As Double
    Public CalcAdjustment As Double
    Public Adjustments As New bc_om_eventactions
    Public Calculations As New bc_om_eventcalculations

    'Deletion values
    Public DeleteClassId As Long
    Public DeleteFrom As Date = "01jan1901"
    Public DeleteTo As Date = "01jan1901"
    Public RowsDeleted As String

    Public write_mode As Integer = 0
    Public Const INSERT = 0
    Public Const UPDATE = 1
    Public Const DELETE = 2

    'Fair Value
    Public FairValue As Double
    Public CalcfvOldMcap As Double
    Public CalcfvAdjusted As Double
    Public CalcfvAdjustment As Double


    Public Sub New(ByVal classid As Long, ByVal entiyid As Long, ByVal eventid As Long, ByVal newshares As Long, ByVal exdate As Date, ByVal price As Double, ByVal ratio As Integer)
        Me.certificate = certificate
        Me.ActionClassId = classid
        Me.ActionEntityId = entiyid
        Me.ActionEventId = eventid
        Me.ActionNewShares = newshares
        Me.ActionExDate = exdate
        Me.ActionPrice = price
        Me.ActionRatio = ratio

    End Sub

    Public Sub New(ByVal actionid As Long)
        Me.ActionId = actionid
    End Sub

    Public Sub New()

    End Sub

    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_corp_action", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Select Case Me.tmode
                Case tWRITE
                    db_write()
                Case tREAD
                    db_read()
            End Select
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_corp_action", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_corp_action", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

        End Try
    End Sub


    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_om_corp_events", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try

            Dim vActionTypes As Object
            Dim dbAction As New bc_om_at_action_db

            vActionTypes = dbAction.read_action_details(Me.ActionId, MyBase.certificate)

            CalcNewShares = vActionTypes(0, 0)
            CalcCapIncreased = vActionTypes(1, 0)
            CalcStockPrice = vActionTypes(2, 0)
            CalcCashDividend = vActionTypes(3, 0)
            CalcAdjustment = vActionTypes(4, 0)
            EventDescriprion = vActionTypes(5, 0)
            EntityDescription = vActionTypes(6, 0)
            ActionEntityId = vActionTypes(7, 0)
            ActionEventId = vActionTypes(8, 0)
            ActionNewShares = vActionTypes(9, 0)
            ActionExDate = vActionTypes(10, 0)
            ActionPrice = vActionTypes(11, 0)
            ActionRatio = vActionTypes(12, 0)
            Submitted = vActionTypes(13, 0)
            StartShares = vActionTypes(14, 0)
            StartMcap = vActionTypes(15, 0)
            StartPrice = vActionTypes(16, 0)
            ActionClassId = vActionTypes(17, 0)

            REM Fair Value
            FairValue = vActionTypes(18, 0)
            CalcfvOldMcap = vActionTypes(19, 0)
            CalcfvAdjusted = vActionTypes(20, 0)
            CalcfvAdjustment = vActionTypes(21, 0)

            REM adjustments to be put in the workbook
            Adjustments.db_read(ActionEventId, "ADJM", ActionClassId, certificate)

            REM calculations
            Calculations.db_read(Me.ActionId, certificate)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_corp_events", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_corp_events", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub

    Public Sub db_write()
        Dim otrace As New bc_cs_activity_log("bc_om_corp_action", "db_write", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try

            Dim dbAction As New bc_om_at_action_db
            Dim doCalculations As Boolean = False
            Dim entityDesc As String
            Dim vDeleteRows As Object

            MyBase.certificate = certificate

            Select Case write_mode
                Case UPDATE
                    If Me.ActionId = 0 Then
                        doCalculations = True
                    End If
                    Dim str As New bc_cs_string_services(Me.EntityDescription)
                    entitydesc = str.delimit_apostrophies

                    ActionId = dbAction.write_action(Me.ActionId, Me.ActionClassId, Me.ActionEntityId, entityDesc, Me.ActionEventId, Me.ActionNewShares, Me.ActionExDate, Me.ActionPrice, Me.ActionRatio, Me.Submitted, Me.CalcNewShares, Me.CalcCapIncreased, Me.CalcStockPrice, Me.CalcCashDividend, Me.CalcAdjustment, Me.DeleteClassId, Me.DeleteFrom, Me.DeleteTo, Me.FairValue, Me.CalcfvOldMcap, Me.CalcfvAdjusted, Me.CalcfvAdjustment, MyBase.certificate)

                Case DELETE

                    Dim str As New bc_cs_string_services(Me.EntityDescription)
                    entityDesc = str.delimit_apostrophies

                    Me.RowsDeleted = ""
                    vDeleteRows = dbAction.delete_time_series(Me.ActionId, Me.ActionClassId, Me.ActionEntityId, entityDesc, Me.ActionEventId, Me.DeleteClassId, Me.DeleteFrom, Me.DeleteTo, MyBase.certificate)
                    Me.RowsDeleted = vDeleteRows(0, 0)


            End Select

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_corp_action", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_corp_action", "db_write", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub

End Class


Public Class bc_om_at_action_db
    Private gbc_db As New bc_cs_db_services
    REM write a tempate record and retuns Id
    Public Sub New()

    End Sub

    REM create a new corp action
    Public Function write_action(ByVal actionid As Long, ByVal classid As Long, ByVal entiyid As Long, ByVal entitydescription As String, ByVal eventid As Long, ByVal newshares As Long, ByVal exdate As Date, ByVal price As Double, ByVal ratio As Integer, ByVal submitted As Integer, ByVal calcnewshares As Long, ByVal calccapincreased As Long, ByVal calcstockprice As Double, ByVal calccashdividend As Double, ByVal calcadjustment As Double, ByVal deleteclassid As Integer, ByVal deletefrom As Date, ByVal deleteto As Date, ByVal fairvalue As Double, ByVal calcfvoldmcap As Double, ByVal calcfvadjusted As Double, ByVal calcfvadjustment As Double, ByRef certificate As bc_cs_security.certificate) As Long
        Dim sql As String
        Dim vres As Object
        Dim id As Long

        sql = "bc_corp_writeaction " + CStr(actionid) + ","
        sql = sql + CStr(classid) + ","
        sql = sql + CStr(entiyid) + ","
        sql = sql + "'" + CStr(entitydescription) + "',"
        sql = sql + CStr(eventid) + ","
        sql = sql + CStr(newshares) + ","
        sql = sql + "'" + Format(exdate, "ddMMMyyyy") + "',"
        sql = sql + CStr(price) + ","
        sql = sql + CStr(ratio) + ","
        sql = sql + CStr(calcnewshares) + ","
        sql = sql + CStr(calccapincreased) + ","
        sql = sql + CStr(calcstockprice) + ","
        sql = sql + CStr(calccashdividend) + ","
        sql = sql + CStr(calcadjustment) + ","
        sql = sql + CStr(submitted) + ","
        sql = sql + CStr(deleteclassid) + ","
        If deletefrom <> "01jan1901" Then
            sql = sql + "'" + Format(deletefrom, "ddMMMyyyy") + "',"
        Else
            sql = sql + "'',"
        End If
        If deleteto <> "01jan1901" Then
            sql = sql + "'" + Format(deleteto, "ddMMMyyyy") + "'"
        Else
            sql = sql + "''"
        End If

        'Fair value
        sql = sql + "," + CStr(calcfvoldmcap)
        sql = sql + "," + CStr(calcfvadjusted)
        sql = sql + "," + CStr(calcfvadjustment)

        'sql = "insert into pub_type_table(pub_type_id,pub_type_name, pub_type_bus_area_id,comment,user_id) select coalesce(max(pub_type_id),0) + 1,'" + name + "'," + CStr(bus_area_id) + ",convert(varchar(20),getdate())," + CStr(certificate.user_id) + " from pub_type_table"
        gbc_db.executesql(sql, certificate)
        sql = "select max(actionid) from bc_corp_actiondetails"
        vres = gbc_db.executesql(sql, certificate)
        If IsArray(vres) Then
            id = vres(0, 0)
        End If
        write_action = id
    End Function


    REM create a new corp action
    Public Function read_action_details(ByVal actionid As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        Dim vres As Object

        sql = "bc_corp_getaction " + CStr(actionid)
        vres = gbc_db.executesql(sql, certificate)

        read_action_details = vres
    End Function

    REM Delete all the data for a market
    Public Function delete_time_series(ByVal actionid As Long, ByVal classid As Long, ByVal entiyid As String, ByVal entitydescription As String, ByVal eventid As Long, ByVal deleteclassid As Long, ByVal fromdate As Date, ByVal todate As Date, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        Dim vres As Object

        sql = "exec dbo.bc_corp_delete_time_series " + CStr(actionid) + "," + CStr(classid) + "," + CStr(entiyid) + ",'" + CStr(entitydescription) + "'," + CStr(eventid) + "," + CStr(deleteclassid) + ",'" + Format(fromdate, "yyyyMMdd") + "','" + Format(todate, "yyyyMMdd") + "'"
        vres = gbc_db.executesql(sql, certificate)

        delete_time_series = vres

    End Function


    'REM do calculations
    'Public Function calculatresults(ByVal actionid As Long, ByRef certificate As bc_cs_security.certificate) As Boolean
    '    Dim sql As String

    '    sql = "bc_corp_calculation " + CStr(actionid)
    '    gbc_db.executesql(sql, certificate)
    '    calculatresults = True

    'End Function

End Class


<Serializable()> Public Class bc_om_corp_events
    Inherits bc_cs_soap_base_class

    Public ActionType As New ArrayList


    Public Overrides Sub process_object()
        Select Case tmode
            Case bc_cs_soap_base_class.tREAD
                db_read()
        End Select
    End Sub


    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_om_corp_events", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try

            Dim i As Integer

            Dim vActionTypes As Object
            Dim oActionType As bc_om_corp_event
            Dim dbCorpEvent As New bc_om_at_corp_event_db

            ActionType.Clear()

            vActionTypes = dbCorpEvent.ReadAllCorpEvents(MyBase.certificate)

            If IsArray(vActionTypes) Then
                For i = 0 To UBound(vActionTypes, 2)
                    oActionType = New bc_om_corp_event(vActionTypes(0, i), vActionTypes(1, i), vActionTypes(2, i), vActionTypes(3, i), vActionTypes(4, i), MyBase.certificate)
                    'oactiontype.comment = vpubtypes(9, i)
                    ActionType.Add(oActionType)
                    ActionType(i).db_read(vActionTypes(0, i), MyBase.certificate)
                Next
            End If


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_corp_events", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_corp_events", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub

End Class


<Serializable()> Public Class bc_om_corp_event
    Inherits bc_cs_soap_base_class

    Public Id As Long
    Public Code As String
    Public Description As String
    Public ClassId As Long
    Public EventType As String
    Public DataInputs As New bc_om_inputs

    Public Sub New(ByVal iid As Integer, ByVal strCode As String, ByVal strDescription As String, ByVal actionClassId As Long, ByVal strEventType As String, ByVal certificate As bc_cs_security.certificate)
        Me.certificate = certificate
        Id = iid
        Code = strCode
        Description = strDescription
        ClassId = actionClassId
        EventType = strEventType
        'Dim datainputs As New bc_om_at_inputs_db
    End Sub
    Public Sub New()

    End Sub

    Public Sub update(ByVal id As Integer, ByVal strName As String, ByVal strDescription As String, ByVal lParentCategory As Long, ByVal lChildCategory As Long, ByVal iLanguage As Integer)
        Id = id
        Code = strName
        Description = strDescription
    End Sub

    Public Sub db_read(ByVal id As Long, ByRef certificate As bc_cs_security.certificate)
        Try
            Dim gdb As New bc_om_at_corp_event_db

            REM form inputs
            DataInputs.db_read(id, certificate)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_corp_event", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try

    End Sub
End Class


Public Class bc_om_at_corp_event_db
    Private gbc_db As New bc_cs_db_services
    REM write a tempate record and retuns Id
    Public Sub New()

    End Sub

    REM reads all corp actions in the database
    Public Function ReadAllCorpEvents(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "bc_corp_getevents"
        ReadAllCorpEvents = gbc_db.executesql(sql, certificate)
    End Function

End Class


<Serializable()> Public Class bc_om_inputs
    Inherits bc_cs_soap_base_class
    Public input As New ArrayList
    Public Sub New()

    End Sub

    Public Sub db_read(ByVal id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim otrace As New bc_cs_activity_log("bc_om_inputs", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try
            Dim i As Integer
            Dim vinputs As Object
            Dim oinput As bc_om_input
            Dim db_input As New bc_om_at_inputs_db
            MyBase.certificate = certificate
            vinputs = db_input.read_inputs(id, MyBase.certificate)
            If IsArray(vinputs) Then
                For i = 0 To UBound(vinputs, 2)
                    oinput = New bc_om_input(vinputs(0, i), vinputs(1, i), vinputs(2, i), vinputs(3, i))

                    oinput.inputcode = vinputs(4, i)
                    oinput.inputtype = vinputs(5, i)

                    input.Add(oinput)
                Next
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_inputs", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_inputs", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub

End Class


<Serializable()> Public Class bc_om_input
    Inherits bc_cs_soap_base_class

    Public Id As Integer
    Public idiinput As Long
    Public fieldno As Long
    Public control As String
    Public inputcode As String
    Public inputtype As String

    Public Sub New()

    End Sub
    Public Sub New(ByVal iid As Integer, ByVal strinput_ininput As Integer, ByVal strinput_fieldno As Long, ByVal strinput_control As String)
        Id = iid
        idiinput = strinput_ininput
        fieldno = strinput_fieldno
        control = strinput_control
    End Sub

    Public Sub db_read()

    End Sub

End Class


Public Class bc_om_at_inputs_db
    Private gbc_db As New bc_cs_db_services
    REM write a tempate record and retuns Id
    Public Sub New()

    End Sub

    REM reads all inputs for a corp action
    Public Function read_inputs(ByVal corp_action_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "bc_corp_getinputs " + CStr(corp_action_id)
        read_inputs = gbc_db.executesql(sql, certificate)
    End Function


End Class



<Serializable()> Public Class bc_om_eventactions
    Inherits bc_cs_soap_base_class
    REM Details of actions to be taken loaded from static data
    Public eventactions As New ArrayList

    Public Sub New()

    End Sub

    Public Sub db_read(ByVal id As Long, ByVal actiontype As String, ByVal actionclass As Long, ByRef certificate As bc_cs_security.certificate)
        Dim otrace As New bc_cs_activity_log("bc_om_eventactions", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try
            Dim i As Integer
            Dim vinputs As Object
            Dim oinput As bc_om_eventaction
            Dim db_eventaction As New bc_om_at_eventactions_db
            MyBase.certificate = certificate
            vinputs = db_eventaction.read_eventactions(id, actiontype, actionclass, MyBase.certificate)
            If IsArray(vinputs) Then
                For i = 0 To UBound(vinputs, 2)
                    oinput = New bc_om_eventaction(vinputs(0, i), vinputs(1, i), vinputs(2, i), vinputs(3, i))

                    oinput.actionorder = vinputs(4, i)
                    oinput.adjustshort = vinputs(5, i)
                    oinput.adjusttemplate = vinputs(6, i)
                    oinput.loadtemplate = vinputs(7, i)
                    oinput.displaytemplate = vinputs(8, i)
                    oinput.adjusttype = vinputs(9, i)
                    oinput.datecolumn = vinputs(10, i)
                    oinput.datatype = vinputs(11, i)
                    eventactions.Add(oinput)
                Next
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_eventactions", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_eventactions", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub

End Class


<Serializable()> Public Class bc_om_eventaction
    Inherits bc_cs_soap_base_class

    Public Id As Integer
    Public eventid As Long
    Public actiontype As String
    Public actionlinkid As String
    Public actionorder As Long
    Public adjustshort As String
    Public adjusttemplate As String
    Public loadtemplate As String
    Public displaytemplate As String
    Public adjusttype As Char
    Public datecolumn As Long
    Public datatype As String

    Public Sub New()

    End Sub
    Public Sub New(ByVal iid As Integer, ByVal strinput_eventid As Integer, ByVal strinput_actiontype As String, ByVal strinput_actionlinkid As String)
        Id = iid
        eventid = strinput_eventid
        actiontype = strinput_actiontype
        actionlinkid = strinput_actionlinkid
    End Sub

    Public Sub db_read()

    End Sub

End Class


Public Class bc_om_at_eventactions_db
    Private gbc_db As New bc_cs_db_services
    REM write a tempate record and retuns Id
    Public Sub New()

    End Sub

    REM reads all inputs for a corp action
    Public Function read_eventactions(ByVal corp_action_id As Long, ByVal corp_eventaction_code As String, ByVal class_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "bc_corp_geteventactions " + CStr(corp_action_id) + ",'" + corp_eventaction_code + "'," + CStr(class_id)
        read_eventactions = gbc_db.executesql(sql, certificate)
    End Function
End Class


<Serializable()> Public Class bc_om_eventcalculations
    Inherits bc_cs_soap_base_class
    REM Details of actions to be taken loaded from static data
    Public eventcalculations As New ArrayList

    Public Sub New()

    End Sub

    Public Sub db_read(ByVal actionid As Long, ByRef certificate As bc_cs_security.certificate)
        Dim otrace As New bc_cs_activity_log("bc_om_eventcalculations", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try
            Dim i As Integer
            Dim vinputs As Object
            Dim oinput As bc_om_eventcalculation
            Dim db_eventaction As New bc_om_at_calculations_db
            MyBase.certificate = certificate
            vinputs = db_eventaction.read_eventcalculations(actionid, MyBase.certificate)
            If IsArray(vinputs) Then
                For i = 0 To UBound(vinputs, 2)
                    oinput = New bc_om_eventcalculation(vinputs(0, i), vinputs(1, i), vinputs(2, i))

                    oinput.adjformula = vinputs(3, i)
                    oinput.targetrow = vinputs(4, i)
                    oinput.targetcolumn = vinputs(5, i)
                    eventcalculations.Add(oinput)
                Next
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_eventcalculations", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_eventcalculations", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub

End Class


<Serializable()> Public Class bc_om_eventcalculation
    Inherits bc_cs_soap_base_class

    Public actionlinkid As String
    Public actionorder As Integer
    Public adjtype As String
    Public adjformula As String
    Public targetrow As Integer
    Public targetcolumn As Integer

    Public Sub New(ByVal iid As String, ByVal strinput_actionorder As Integer, ByVal strinput_adjtype As String)
        actionlinkid = iid
        actionorder = strinput_actionorder
        adjtype = strinput_adjtype
    End Sub

    Public Sub db_read()

    End Sub

End Class


Public Class bc_om_at_calculations_db
    Private gbc_db As New bc_cs_db_services
    REM write a tempate record and retuns Id
    Public Sub New()

    End Sub

    REM reads all inputs for a corp action
    Public Function read_eventcalculations(ByVal corp_action_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "bc_corp_get_calculations " + CStr(corp_action_id)
        read_eventcalculations = gbc_db.executesql(sql, certificate)
    End Function
End Class

<Serializable()> Public Class bc_om_period_headers
    Inherits bc_cs_soap_base_class
    REM Details of actions to be taken loaded from static data
    Public periodheadings As New ArrayList
    Public entityname As String

    Public Sub New()

    End Sub

    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_corp_action", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Select Case Me.tmode
                Case tWRITE
                    db_write()
                Case tREAD
                    db_read()
            End Select
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_corp_action", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_corp_action", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

        End Try
    End Sub

    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_om_period_headers", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try
            Dim i As Integer
            Dim vinputs As Object
            Dim oinput As bc_om_period
            Dim db_eventaction As New bc_om_at_period_headers_db
            MyBase.certificate = certificate
            vinputs = db_eventaction.read_headers(entityname, MyBase.certificate)
            If IsArray(vinputs) Then
                For i = 0 To UBound(vinputs, 2)
                    oinput = New bc_om_period(vinputs(0, i), vinputs(1, i), vinputs(2, i))
                    oinput.periodeaflag = vinputs(3, i)

                    periodheadings.Add(oinput)
                Next
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_period_headers", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_period_headers", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub

    Public Sub db_write()

    End Sub

End Class

Public Class bc_om_at_period_headers_db
    Private gbc_db As New bc_cs_db_services
    Public Sub New()

    End Sub

    REM read period headers
    Public Function read_headers(ByVal corp_entity_name As String, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "bc_corp_getperiodheaders '" + corp_entity_name + "'"
        read_headers = gbc_db.executesql(sql, certificate)
    End Function
End Class

<Serializable()> Public Class bc_om_period
    Inherits bc_cs_soap_base_class

    Public periodyear As Integer
    Public periodname As String
    Public periodsequence As Integer
    Public periodeaflag As String
   

    Public Sub New(ByVal year As Integer, ByVal name As String, ByVal sequence As Integer)
        periodyear = year
        periodname = name
        periodsequence = sequence

    End Sub

    Public Sub db_read()

    End Sub

End Class