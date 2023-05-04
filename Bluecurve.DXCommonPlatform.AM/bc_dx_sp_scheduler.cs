using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using BlueCurve.Core.CS;
using BlueCurve.Core.OM;
using BlueCurve.CommonPlatform.AM;
namespace Bluecurve.DXCommonPlatform.AM
{
    public partial class bc_dx_sp_scheduler : DevExpress.XtraBars.Ribbon.RibbonForm, Ibc_dx_task_scheduler
    {
        public bc_dx_sp_scheduler()
        {
            InitializeComponent();
        }

        public event EventHandler<EventArgs> refresh_view;
        public event EventHandler<Etaskargs> set_enable;
        public event EventHandler<EventArgs> new_task;
        public event EventHandler<Etaskargs> maintain_task;
        public event EventHandler<Etaskargs> load_history;
        public event EventHandler<Etaskargs> delete_task;

        bc_om_dl_tasks _tasks= new  bc_om_dl_tasks();




        public bool load_view(bc_om_dl_tasks tasks, bool read_only) 
    {
        try
        {
            ribbonPage2.Visible = false;

            uxtasks.FocusedNodeChanged += uxtasks_FocusedNodeChanged;
            timer1.Tick += timer1_tick;
            RepositoryItemComboBox2.SelectedIndexChanged += RepositoryItemComboBox2_SelectedIndexChanged;
            uxtasks.DoubleClick += uxtasks_DoubleClick;

            _tasks = tasks;
            load_task_tree();
            timer1.Start();
            return true;
        }
         catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_dx_cp_schedule", "get_data", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
                return false;
            }

        }
    


   public bool update_view(bc_om_dl_tasks tasks) 
   { 
        try
        {
            _tasks = tasks;
            load_task_tree();
            return true;
        }
         catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_dx_cp_schedule", "update_view(", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
                return false;
         }
     }


   void timer1_tick(Object sender,EventArgs e)
   {
       EventHandler<EventArgs> handler = refresh_view;
       if (handler != null)
       {
           EventArgs args = new EventArgs();
           handler(this, args);
       }
   }

    void load_task_tree()
    {
        try
        {
            uxtasks.BeginUpdate();
            string sel_row = "";
            if (uxtasks.Nodes.Count > 0)
                sel_row =uxtasks.FocusedNode.Tag.ToString();
           
            
            uxtasks.Nodes.Clear();
            int i;
            for (i = 0; i < _tasks.tasks.Count ; i++)
            {
                lpoll.Text = "Last Updated: " + _tasks.tasks[i].current_datetime.ToLocalTime().ToString("dd MMM yyyy HH:mm:ss");
                uxtasks.Nodes.Add();
                uxtasks.Nodes[uxtasks.Nodes.Count - 1].SetValue(9, _tasks.tasks[i].task_id);
                uxtasks.Nodes[uxtasks.Nodes.Count - 1].SetValue(0, _tasks.tasks[i].task_name);
                if (_tasks.tasks[i].input == true)
                    uxtasks.Nodes[uxtasks.Nodes.Count - 1].SetValue(1, "Input");
                else
                    uxtasks.Nodes[uxtasks.Nodes.Count - 1].SetValue(1, "Output");

                string fstr="";
                String hr;
                String mn;
                String se;
                DateTime da = new DateTime(9999, 9, 9, _tasks.tasks[i].hour, 0, 0);
                hr = da.ToLocalTime().Hour.ToString();
                mn = _tasks.tasks[i].minute.ToString();
                se = _tasks.tasks[i].second.ToString();
                if (hr.Length == 1)
                    hr = "0" + hr;

                if (mn.Length == 1)
                    mn = "0" + mn;

                if (se.Length == 1)
                    se = "0" + se;

                switch (_tasks.tasks[i].frequency_type)
                {
                    case 1:
                        fstr = "Day at " + hr + ":" + mn + ":" + se;
                        break;
                    case 2:
                        fstr = "Hour(s) at " + mn + " minutes & " + se + " seconds past the hour";
                        break;
                    case 3:
                        fstr = "Minute(s) at " + se + " seconds past the minute";
                        break;
                    case 4:
                        fstr = "Second(s)";
                        break;
                }


                fstr = "Every " + _tasks.tasks[i].interval.ToString() + " " + fstr;
                uxtasks.Nodes[uxtasks.Nodes.Count - 1].SetValue(2, fstr);


                if (_tasks.tasks[i].status == bc_om_dl_tasks.ETASK_STATUS.SERVICING)
                {
                  uxtasks.Nodes[uxtasks.Nodes.Count - 1].SetValue(3, _tasks.tasks[i].start_time.ToLocalTime().ToString("dd MMM yyyy HH:mm:ss"));
                  uxtasks.Nodes[uxtasks.Nodes.Count - 1].SetValue(4, _tasks.tasks[i].last_run.ToLocalTime().ToString("dd MMM yyyy HH:mm:ss"));
                }
                else if (_tasks.tasks[i].status == bc_om_dl_tasks.ETASK_STATUS.NOT_YET_RUN) 
                {
                  uxtasks.Nodes[uxtasks.Nodes.Count - 1].SetValue(3, _tasks.tasks[i].start_time.ToLocalTime().ToString( "dd MMM yyyy HH:mm:ss"));
                  if (_tasks.tasks[i].disabled == false )
                  {
                    uxtasks.Nodes[uxtasks.Nodes.Count - 1].SetValue(5,_tasks.tasks[i].next_run.ToLocalTime().ToString( "dd MMM yyyy HH:mm:ss"));
                  }
                }
                  else
                  {
                    uxtasks.Nodes[uxtasks.Nodes.Count - 1].SetValue(3, _tasks.tasks[i].start_time.ToLocalTime().ToString( "dd MMM yyyy HH:mm:ss"));
                    uxtasks.Nodes[uxtasks.Nodes.Count - 1].SetValue(4,_tasks.tasks[i].last_run.ToLocalTime().ToString( "dd MMM yyyy HH:mm:ss"));
                    if (_tasks.tasks[i].disabled == false)
                    {
                        uxtasks.Nodes[uxtasks.Nodes.Count - 1].SetValue(5, _tasks.tasks[i].next_run.ToLocalTime().ToString("dd MMM yyyy HH:mm:ss"));
                    }
                  }
                
                switch (_tasks.tasks[i].status)
                  {
                            case bc_om_dl_tasks.ETASK_STATUS.NOT_YET_RUN:
                                uxtasks.Nodes[uxtasks.Nodes.Count - 1].SetValue(6, "Not yet run");
                                uxtasks.Nodes[uxtasks.Nodes.Count - 1].StateImageIndex = -1;
                                    break;
                            case bc_om_dl_tasks.ETASK_STATUS.SUCCESS:
                                uxtasks.Nodes[uxtasks.Nodes.Count - 1].SetValue(6, "Success");
                                uxtasks.Nodes[uxtasks.Nodes.Count - 1].StateImageIndex = 1;
                                    break;
                            case bc_om_dl_tasks.ETASK_STATUS.FAIL:
                                uxtasks.Nodes[uxtasks.Nodes.Count - 1].SetValue(6, "Failed");
                                uxtasks.Nodes[uxtasks.Nodes.Count - 1].StateImageIndex = 0;
                                    break;
                            case bc_om_dl_tasks.ETASK_STATUS.SERVICING:
                                uxtasks.Nodes[uxtasks.Nodes.Count - 1].SetValue(6, "Running");
                                uxtasks.Nodes[uxtasks.Nodes.Count - 1].StateImageIndex = 2;
                                    break;
                   }
                   uxtasks.Nodes[uxtasks.Nodes.Count - 1].SetValue(7, _tasks.tasks[i].comment);

                   if (_tasks.tasks[i].disabled == false)
                      uxtasks.Nodes[uxtasks.Nodes.Count - 1].SetValue(8, "True");
                   else
                   {
                      uxtasks.Nodes[uxtasks.Nodes.Count - 1].SetValue(8, "False");
                     uxtasks.Nodes[uxtasks.Nodes.Count - 1].StateImageIndex = 3;
                   }

                   uxtasks.Nodes[uxtasks.Nodes.Count - 1].Tag = _tasks.tasks[i].task_id.ToString();
                }
              
                if (sel_row != "")
                {
                    for (i = 0; i < uxtasks.Nodes.Count; i++)
                    {
                        if (uxtasks.Nodes[i].Tag.ToString() == sel_row)
                        {
                            uxtasks.Nodes[i].Selected = true;
                            break;
                        }
                    }
                }
           
       }
       catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_dx_cp_schedule", "load_task_tree", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            }
   finally
   {
     uxtasks.EndUpdate();
   }
   }




    void uxtasks_FocusedNodeChanged(Object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e) 
    {
        try
        {
            ribbonPage2.Visible = false;


            if (uxtasks.Selection.Count == 1)
            {
                ribbonPage2.Visible = true;
                ribbonPage2.Text = uxtasks.FocusedNode.GetValue(0).ToString();
                ribbon.SelectedPage = ribbonPage2;
            }
        }
        catch
        {

        }
     }

    private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
    {
        EventHandler<EventArgs> handler = refresh_view;
        if (handler != null)
        {
            EventArgs args = new EventArgs();
            handler(this, args);
        }
    }

    private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
    {

    }

    private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
    {
        EventHandler<Etaskargs> handler = maintain_task;

        if (handler != null)
        {
            Etaskargs args = new Etaskargs();
            int i;
            for (i = 0; i < _tasks.tasks.Count - 1; i++)
            {
                if (_tasks.tasks[i].task_id == (int)uxtasks.FocusedNode.GetValue(9))
                {
                    args.task = _tasks.tasks[i];
                    handler(this, args);
                    break;
                }
            }
        }
    }

    private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
    {
        EventHandler<Etaskargs> handler = delete_task;
        if (handler != null)
        {
            Etaskargs args = new Etaskargs();
            args.task_id = (int)uxtasks.FocusedNode.GetValue(9);
            handler(this, args);
        }
    }

    private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
    {
        EventHandler<Etaskargs> handler = load_history;
        if (handler != null)
        {
            int i;
            for (i = 0; i < _tasks.tasks.Count - 1; i++)
            {
                if (_tasks.tasks[i].task_id == (int)uxtasks.FocusedNode.GetValue(9))
                {
                    Etaskargs args = new Etaskargs();
                    args.task_id = _tasks.tasks[i].task_id;
                    args.task_name=_tasks.tasks[i].task_name;
                    handler(this, args);
                    break;
                }
            }
        }
    }

    private void barButtonItem2_ItemClick_1(object sender, ItemClickEventArgs e)
    {
        EventHandler<EventArgs> handler = new_task;
        EventArgs args = new EventArgs();
        handler(this, args);
    }


    


    void load_comment()
    {
        try
        {
            bc_dx_html_task_comment fc = new bc_dx_html_task_comment();
            Cbc_dx_html_task_comment cc = new Cbc_dx_html_task_comment();
            if (uxtasks.FocusedNode.GetValue(7).ToString()!="")
              if (cc.load_data(fc, uxtasks.FocusedNode.GetValue(7).ToString(), uxtasks.FocusedNode.GetValue(0).ToString()) == true)
                fc.ShowDialog();
        }
        catch
        {

        }
        
    }

       void uxtasks_DoubleClick(object sender, EventArgs e)
       {
           load_comment();
       }



   void RepositoryItemComboBox2_SelectedIndexChanged(object sender, EventArgs e)
   {
       
        EventHandler<Etaskargs> handler = set_enable;
        Etaskargs args = new Etaskargs();
        args.task_id=(int)uxtasks.FocusedNode.GetValue(9);
        if (uxtasks.FocusedNode.GetValue(8).ToString() == "True")
            args.enable = 0;
        else
            args.enable = 1;

        handler(this, args);
   }
}



public class Cbc_dx_cp_scheduler
{
    Ibc_dx_task_scheduler _view;
    bc_om_dl_tasks _tasks = new bc_om_dl_tasks();
    public bool  load_data(Ibc_dx_task_scheduler view,bool read_only)
   {
        //load_data = False
        try
        {
            _view = view;
            _view.refresh_view +=refresh_view;
            _view.set_enable +=set_enable;
            _view.new_task += new_task;
            _view.maintain_task +=maintain_task;
            _view.load_history +=load_history;
            _view.delete_task +=delete_task;

            if (get_data() == true)
                return _view.load_view(_tasks, read_only);
            else
                return false;
        }
        catch (Exception ex)
        {
            bc_cs_security.certificate certificate = new bc_cs_security.certificate();
            bc_cs_error_log oerr = new bc_cs_error_log("Cbc_dx_cp_schedule", "load_data", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
            return false;
        }
    }

    bool get_data() 
    {

        try
        {
            if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                _tasks.db_read();
            else
            {
                _tasks.tmode = bc_cs_soap_base_class.tREAD;
                object otasks;
                otasks= (object)_tasks;

                if (_tasks.transmit_to_server_and_receive(ref  otasks , true) == false)
                    return false;
                _tasks = (bc_om_dl_tasks)otasks;
            }
            return true;
        }
         catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("Cbc_dx_cp_schedule", "get_data", bc_cs_error_codes.USER_DEFINED.ToString(), ex.Message.ToString(), ref certificate);
                return false;
            }
    }
    void refresh_view(object sender, EventArgs e) 
    {
        if (get_data() == true)
            _view.update_view(_tasks);
    }
    void new_task(object sender, EventArgs e) 
    {
        bc_dx_maintain_task fmt = new bc_dx_maintain_task();
        Cbc_dx_maintain_task cmt = new Cbc_dx_maintain_task();
        if (cmt.load_data(fmt, null) == true)
            fmt.ShowDialog();

        refresh_view(null,null);
    }

    
    void maintain_task(object sender, Etaskargs e)
    {
        bc_dx_maintain_task fmt = new bc_dx_maintain_task();
        Cbc_dx_maintain_task cmt = new Cbc_dx_maintain_task();
        if (cmt.load_data(fmt, e.task) == true)
            fmt.ShowDialog();

        refresh_view(null, null);
    }


    void load_history(object sender, Etaskargs e)
    {
        bc_dx_task_history fhi = new bc_dx_task_history();
        Cbc_dx_task_history chi = new Cbc_dx_task_history();
           
        if (chi.load_data(fhi, e.task_id, e.task_name) == true)
                fhi.ShowDialog();
            
        refresh_view(null, null);
    }

    void delete_task(object sender, Etaskargs e)
    {
        bc_om_dl_tasks.bc_om_dl_task task = new bc_om_dl_tasks.bc_om_dl_task();
        task.task_id = e.task_id;
        task.tmode = bc_cs_soap_base_class.tWRITE;
        task.write_mode = bc_om_dl_tasks.bc_om_dl_task.Ewrite_mode.delete;
       
        if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
            task.db_write();
        else
        {
            task.tmode = bc_cs_soap_base_class.tREAD;
            object otasks;
            otasks = (object)_tasks;

            if (task.transmit_to_server_and_receive(ref  otasks, true) == false)
                return;
            task = (bc_om_dl_tasks.bc_om_dl_task)otasks;

        }
       refresh_view(null, null);
    }
    void set_enable(object sender, Etaskargs e)
    {
        bc_om_dl_tasks.bc_om_dl_task task = new bc_om_dl_tasks.bc_om_dl_task();
        task.task_id = e.task_id;
       
        if (e.enable == 1)
            task.write_mode = bc_om_dl_tasks.bc_om_dl_task.Ewrite_mode.enable;
        else
            task.write_mode = bc_om_dl_tasks.bc_om_dl_task.Ewrite_mode.disable;
        
        if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
            task.db_write();
        else
        {
            task.tmode = bc_cs_soap_base_class.tWRITE;
            object otasks;
            otasks = (object)task;

            if (task.transmit_to_server_and_receive(ref  otasks, true) == false)
                return;
           
        }
        refresh_view(null, null);
    }

}
public interface Ibc_dx_task_scheduler
{
     bool load_view(bc_om_dl_tasks tasks, bool read_only);
     bool update_view(bc_om_dl_tasks tasks);
     event EventHandler<EventArgs> refresh_view;
     event EventHandler<Etaskargs> set_enable;
     event EventHandler<EventArgs> new_task;
     event EventHandler<Etaskargs> maintain_task;
     event EventHandler<Etaskargs> load_history;
     event EventHandler<Etaskargs> delete_task;
}
public class Etaskargs : EventArgs
{
        public int task_id { get; set; }
        public int enable { get; set; }
        public string task_name  { get; set; }
        public  bc_om_dl_tasks.bc_om_dl_task task  { get; set; }
}
}