Public Module bc_am_dd_enums

    Public Enum MaskType
        Blank = 0
        Include = 1
        Archive = 2
        Exclude = 3
        Display = 4
    End Enum

    Public Enum UriPickerType
        File = 0
        Folder = 1
    End Enum

    Public Enum QueryType
        UPDATE = 1
        DELETE = 2
        INSERT = 3
    End Enum

    Public Enum ErrorCodes
        NONE = 0
        LOAD = 1
        EXPORT = 2
        COMPARE = 3
        IMPORT = 4
        LOCK = 5
        UNLOCK = 6
        UNKNOWN = 7
    End Enum

End Module
