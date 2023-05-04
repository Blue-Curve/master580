using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bc_core_components_objects
{
    public enum SYSTEM_DEFINED{NONE=0,SET=1,RUNTIME=2};
   
  
    public class bc_core_table
    {
        List<bc_core_row> _rows = new List<bc_core_row>();

        public List<bc_core_row> rows
        {
            get { return _rows; }
        }
        public bc_core_table()
        {

        }
        public void add_row(bc_core_row row)
        {
            _rows.Add(row);
        }


    }
    public class bc_core_row
    {
        List<bc_core_cell> _cells = new List<bc_core_cell>();
        public List<bc_core_cell> cells
        {
            get { return _cells; }

        }

        public bc_core_row()
        {

        }
        public void add_cell(bc_core_cell cell)
        {
             _cells.Add(cell);
        }
    }
    public class bc_core_cell
    {
        List< bc_core_cell_paragraph> _paragraphs;
        public List<bc_core_cell_paragraph> paragraphs
        {
            get { return _paragraphs; }
            set { _paragraphs = value; }
        }
      
        public bc_core_cell( List< bc_core_cell_paragraph> paragraphs)
        {
            _paragraphs=paragraphs;
        }
    }

    public class bc_core_cell_paragraph
         {
        string _value;
        string _style;
        bool _image;

        public string value
        {
            get { return _value; }
        }
        public string style
        {
            get { return _style; }
        }
        public bool image
        {
            get { return _image; }
        }
        public bc_core_cell_paragraph(string value, string style, bool image)
        {
            _value = value;
            _style = style;
            _image = image;
        }
    }

    public class bc_core_document
    {
        long _doc_id;
        long _template_id;
        List<bc_core_lead_paragraph> _lead_paragraphs;

        public long doc_id
        {
            get { return _doc_id; ; }
            set {_doc_id = value; }
        }
        public long template_id
        {
            get { return _template_id; }
            set { _template_id = value;  }
        }
        public List<bc_core_lead_paragraph> lead_paragraphs
        {
            get { return _lead_paragraphs; }
            set { _lead_paragraphs = value; }
        }
    }
    public class bc_core_lead_paragraph : bc_core_base_paragraph
    {
        List<bc_core_paragraph>  _paragraphs;
        int _text_box_id;
        public List<bc_core_paragraph> paragraphs
        {
            get { return _paragraphs; ; }
            set { _paragraphs = value; }
        }
        public int text_box_id
        {
            get { return _text_box_id; ; }
            set { _text_box_id = value; }
        }
      
    }
    public class bc_core_paragraph : bc_core_base_paragraph
    {
      
        
    }
    public  abstract class bc_core_base_paragraph
    {
        string _text;
        string _style;
        int _page_number;
        bool _is_table;
        bool _is_image;
        bc_core_table _table;
        int _display_order;
        string _desc;

        public  bc_core_table table
        {
            get { return _table; }
            set { _table = value; }
        }

        public string text
        {
            get { return _text; }
            set { _text = value; }
        }
        public string style
        {
            get { return _style; }
            set { _style = value; }
        }
        public int page_number
        {
            get { return _page_number; }
            set { _page_number = value; }
        }
        public bool is_table
        {
            get { return _is_table; }
            set { _is_table = value; }
        }
        public bool is_image
        {
            get { return _is_image; }
            set { _is_image = value; }
        }
        public string desc
        {
            get { return _desc; }
            set { _desc = value; }
        }
        public int  display_order
         {
            get { return _display_order; }
            set { _display_order = value; }
        }

    }
}