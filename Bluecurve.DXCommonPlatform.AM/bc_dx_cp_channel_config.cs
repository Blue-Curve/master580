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
namespace Bluecurve.DXCommonPlatform.AM
{
    public partial class bc_dx_cp_channel_config : DevExpress.XtraBars.Ribbon.RibbonForm, Ibc_dx_cp_channel_config
    {
        public bc_dx_cp_channel_config()
        {
            InitializeComponent();
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }
        bc_om_distribution_channels_config _channels;
  
        public event EventHandler<Eupdatechannelargs> Eupdatechannel;

        public bool load_view(bc_om_distribution_channels_config channels)
        {
            try
            {
                barButtonItem4.Enabled = false;
                _channels = channels;
                uxchannels.BeginUpdate();
                uxchannels.Nodes.Clear();
                int i;
                for (i=0; i < channels.channels.Count;i++)
                {
                    uxchannels.Nodes.Add();
                    uxchannels.Nodes[uxchannels.Nodes.Count - 1].Tag=channels.channels[i].id;
                    uxchannels.Nodes[uxchannels.Nodes.Count - 1].SetValue(0, channels.channels[i].name);
                    switch (channels.channels[i].transfer_method)
                    {
                        case bc_cs_net_send_channel.eTRANSFER_METHOD.REST_POST:
                             uxchannels.Nodes[uxchannels.Nodes.Count - 1].SetValue(1, "Rest Post");
                            break;

                        case bc_cs_net_send_channel.eTRANSFER_METHOD.FTP:
                             uxchannels.Nodes[uxchannels.Nodes.Count - 1].SetValue(1, "Ftp");
                            break;
                        case bc_cs_net_send_channel.eTRANSFER_METHOD.SFTP:
                             uxchannels.Nodes[uxchannels.Nodes.Count - 1].SetValue(1, "Sftp");
                            break;
                        case bc_cs_net_send_channel.eTRANSFER_METHOD.FILE_COPY:
                             uxchannels.Nodes[uxchannels.Nodes.Count - 1].SetValue(1, "File Copy");
                            break;

                    }
                    if (channels.channels[i].generate_mail_list== true)
                      uxchannels.Nodes[uxchannels.Nodes.Count - 1].SetValue(2, "Yes");
                    else
                      uxchannels.Nodes[uxchannels.Nodes.Count - 1].SetValue(2, "No");

                    uxchannels.Nodes[uxchannels.Nodes.Count - 1].SetValue(3, channels.channels[i].files_sp);
                    uxchannels.Nodes[uxchannels.Nodes.Count - 1].SetValue(4, channels.channels[i].uri);

                    uxchannels.Nodes[uxchannels.Nodes.Count - 1].SetValue(5, channels.channels[i].username);

                    uxchannels.Nodes[uxchannels.Nodes.Count - 1].SetValue(6, channels.channels[i].password);

                    uxchannels.Nodes[uxchannels.Nodes.Count - 1].SetValue(7, channels.channels[i].dir);

                    uxchannels.Nodes[uxchannels.Nodes.Count - 1].SetValue(8, channels.channels[i].port);
                    uxchannels.Nodes[uxchannels.Nodes.Count - 1].SetValue(9, channels.channels[i].fingerprint);

                    if (channels.channels[i].compress == true)
                       uxchannels.Nodes[uxchannels.Nodes.Count - 1].SetValue(10,"Yes");
                    else
                       uxchannels.Nodes[uxchannels.Nodes.Count - 1].SetValue(10, "No");



                    uxchannels.Nodes[uxchannels.Nodes.Count - 1].SetValue(11, channels.channels[i].channel_group );
                    if (channels.channels[i].in_use==false)
                      uxchannels.Nodes[uxchannels.Nodes.Count - 1].SetValue(12, "No");
                    else
                      uxchannels.Nodes[uxchannels.Nodes.Count - 1].SetValue(12, "Yes");

                    uxchannels.Nodes[uxchannels.Nodes.Count - 1].StateImageIndex = 1;   



                }
                return true;
            }
            catch (Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_channel_config", "load view", bc_cs_error_codes.USER_DEFINED, ex.Message.ToString(), ref certificate);
                return false;
            }
            finally
            {
                uxchannels.EndUpdate();
            }
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            uxchannels.Nodes.Remove(uxchannels.FocusedNode);
        }

        private void uxchannels_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            barButtonItem4.Enabled = false;
            try
            {
                if (uxchannels.Selection.Count == 1)
                    if (uxchannels.FocusedNode.GetValue(12).ToString() == "No")
                        barButtonItem4.Enabled = true;
            }
            catch
            {

            }
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            uxchannels.Nodes.Add();
            uxchannels.Nodes[uxchannels.Nodes.Count - 1].Tag = 0;
            uxchannels.Nodes[uxchannels.Nodes.Count - 1].SetValue(0,"");
            uxchannels.Nodes[uxchannels.Nodes.Count - 1].SetValue(1, "Ftp");
            uxchannels.Nodes[uxchannels.Nodes.Count - 1].SetValue(2, "No");
            uxchannels.Nodes[uxchannels.Nodes.Count - 1].SetValue(3, "");
            uxchannels.Nodes[uxchannels.Nodes.Count - 1].SetValue(4, "");
            uxchannels.Nodes[uxchannels.Nodes.Count - 1].SetValue(5, "");
            uxchannels.Nodes[uxchannels.Nodes.Count - 1].SetValue(6, "");
            uxchannels.Nodes[uxchannels.Nodes.Count - 1].SetValue(7, "");
            uxchannels.Nodes[uxchannels.Nodes.Count - 1].SetValue(8, "0");
            uxchannels.Nodes[uxchannels.Nodes.Count - 1].SetValue(9, "");
            uxchannels.Nodes[uxchannels.Nodes.Count - 1].SetValue(10, "No");
            uxchannels.Nodes[uxchannels.Nodes.Count - 1].SetValue(11, "1");
            uxchannels.Nodes[uxchannels.Nodes.Count - 1].SetValue(12, "No");

        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                string err_tx="";
                err_tx = "Please correct following errors: ";
                uxchannels.FocusedColumn =uxchannels.Columns[1];
                uxchannels.FocusedColumn = uxchannels.Columns[2];



                bool osuccess = true;
                bool csuccess = true;
                bc_om_distribution_channels_config channels = new bc_om_distribution_channels_config();
                bc_om_distribution_channel_config channel;
                int i;
                for (i = 0; i < uxchannels.Nodes.Count; i++)
                {
                    csuccess = true;
                    channel = new bc_om_distribution_channel_config();
                    uxchannels.Nodes[i].StateImageIndex = 1;

                    channel.id = (int)uxchannels.Nodes[i].Tag;
                    channel.name = uxchannels.Nodes[i].GetValue(0).ToString();
                    int j;
                    for (j = 0; j < uxchannels.Nodes.Count; j++)
                    {
                        if (j!=i && uxchannels.Nodes[j].GetValue(0).ToString()==channel.name)
                        {
                            err_tx = err_tx + Environment.NewLine + " Duplicate Channel Name: " + channel.name;
                            csuccess = false;
                            break;
                        }
                    }


                    if (channel.name.Trim() == "")
                    {
                        err_tx = err_tx + Environment.NewLine + " Channel Name Must Be Entered "; 
                        csuccess = false;
                    }
                   
                    if (uxchannels.Nodes[i].GetValue(1).ToString() == "Ftp")
                            channel.transfer_method = bc_cs_net_send_channel.eTRANSFER_METHOD.FTP;
                    else if (uxchannels.Nodes[i].GetValue(1).ToString() == "Sftp")
                            channel.transfer_method = bc_cs_net_send_channel.eTRANSFER_METHOD.SFTP;
                    else if (uxchannels.Nodes[i].GetValue(1).ToString() == "Rest Post")
                            channel.transfer_method = bc_cs_net_send_channel.eTRANSFER_METHOD.REST_POST;
                   else if (uxchannels.Nodes[i].GetValue(1).ToString() == "File Copy")
                            channel.transfer_method = bc_cs_net_send_channel.eTRANSFER_METHOD.FILE_COPY;

                    if (uxchannels.Nodes[i].GetValue(2).ToString() == "Yes")
                        channel.generate_mail_list = true;
                    else
                        channel.generate_mail_list = false;

                   channel.files_sp = uxchannels.Nodes[i].GetValue(3).ToString();
                   if (channel.files_sp.Trim() == "")
                   {
                       err_tx = err_tx + Environment.NewLine +  " Files SP Name must be entered for " + channel.name;
                       csuccess = false;
                   }

                   if   (channel.transfer_method != bc_cs_net_send_channel.eTRANSFER_METHOD.FILE_COPY)
                   {
                       channel.uri = uxchannels.Nodes[i].GetValue(4).ToString();
                       if (channel.uri.Trim() == "")
                       {
                           err_tx = err_tx + Environment.NewLine + " URI must be entered for " + channel.name;
                           csuccess = false;
                       }
                       if (channel.transfer_method != bc_cs_net_send_channel.eTRANSFER_METHOD.REST_POST)
                       {
                           channel.username = uxchannels.Nodes[i].GetValue(5).ToString();
                           if (channel.username.Trim() == "")
                           {
                               err_tx = err_tx + Environment.NewLine + " Username must be entered for " + channel.name;
                               csuccess = false;
                           }
                           channel.password = uxchannels.Nodes[i].GetValue(6).ToString();
                           if (channel.password.Trim() == "")
                           {
                               err_tx = err_tx + Environment.NewLine + " Password must be entered for " + channel.name;
                               csuccess = false;
                           }
                           channel.dir = uxchannels.Nodes[i].GetValue(7).ToString();
                           try
                           {
                               channel.port = (int)uxchannels.Nodes[i].GetValue(8);
                           }
                           catch
                           {
                               channel.port = 0;
                           }
                       }
                       if (channel.transfer_method == bc_cs_net_send_channel.eTRANSFER_METHOD.SFTP)
                       {
                           channel.fingerprint = uxchannels.Nodes[i].GetValue(9).ToString();
                           if (channel.fingerprint.Trim() == "")
                           {
                               err_tx = err_tx + Environment.NewLine + " Fingerprint must be entered for " + channel.name;
                               csuccess = false;
                           }

                       }

                   }


                    if (csuccess == false)
                    {
                        uxchannels.Nodes[i].StateImageIndex = 0;
                        osuccess = false;
                    }
                    channels.channels.Add(channel);
                }


                if (osuccess ==false)
                {
                    bc_cs_message msg = new bc_cs_message("Blue Curve", err_tx, bc_cs_message.MESSAGE, false, false, "Yes", "No", true);
                    return;
                }


                EventHandler<Eupdatechannelargs> handler = Eupdatechannel;
                if (handler != null)
                {
                    Eupdatechannelargs args = new Eupdatechannelargs();
                    args.channels = channels;
                    handler(this, args);
                }
                Close();
            }
            catch(Exception ex)
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                bc_cs_error_log oerr = new bc_cs_error_log("bc_dx_cp_channel_config", "save", bc_cs_error_codes.USER_DEFINED, ex.Message.ToString(), ref certificate);
            }
        }
    }
    public class Cbc_dx_cp_channel_config
    {
        Ibc_dx_cp_channel_config _view;

        public bool load_data(Ibc_dx_cp_channel_config view)
        {
            _view = view;
            _view.Eupdatechannel += Update_Channel;

            bc_om_distribution_channels_config channels = new bc_om_distribution_channels_config();
            if (bc_cs_central_settings.selected_conn_method== bc_cs_central_settings.ADO)
                channels.db_read();
            else
            {
                channels.tmode = bc_cs_soap_base_class.tREAD;
                object ochannels;
                ochannels = (object)channels;
                 if (channels.transmit_to_server_and_receive(ref ochannels, true) == false)
                    return false;
                channels = (bc_om_distribution_channels_config)ochannels;
            }

            return _view.load_view(channels);
        }
        private void Update_Channel(object sender, Eupdatechannelargs args)
        {
           
            if (bc_cs_central_settings.selected_conn_method == bc_cs_central_settings.ADO)
                args.channels.db_write();
            else
            {
                args.channels.tmode = bc_cs_soap_base_class.tWRITE;
                object ochannels;
                ochannels = (object)args.channels;
               args.channels.transmit_to_server_and_receive(ref ochannels, true);
            }
           
        }
    }
    public interface Ibc_dx_cp_channel_config
    {
        bool load_view(bc_om_distribution_channels_config channels);
        event EventHandler<Eupdatechannelargs> Eupdatechannel;

    }
    public class Eupdatechannelargs : EventArgs
    {
        public bc_om_distribution_channels_config channels{ get; set; }
    }
}