using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BlueCurve.Core.CS;
using DevExpress.XtraTreeList;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Net.NetworkInformation;
namespace BlueCurve.Aggregations.AM
{
    public partial class bc_am_aggs_preview : DevExpress.XtraEditors.XtraForm, Ibc_am_aggs_preview
    {
      
        universes _universes;
        Cbc_am_aggs_preview _contoller;
        List<abc_calc_agg> _abc_calc_agg;
        List<abc_calc_agg> _abc_calc_agg_growths;
        List<abc_calc_agg> _abc_calc_agg_cc;
        List<agg_result> results;
        public bc_am_aggs_preview()
        {
            InitializeComponent();
        }

        public void load_results(BlueCurve.Aggregations.AM.ServiceReference1.agg_results lresults)
        {
            _abc_calc_agg = new List<abc_calc_agg>();
            _abc_calc_agg_growths = new List<abc_calc_agg>();
            _abc_calc_agg_cc = new List<abc_calc_agg>();

            results = new List<agg_result>();
          
            agg_result result;
            abc_calc_agg  abc_calc_agg;
            int i;
            results.Clear();
            for (i = 0; i < lresults.results.Length;i++ )
            {
                result=new agg_result();
                result.value = lresults.results[i].value;
                result.contributor_id = lresults.results[i].contributor_id;
                result.item_id = lresults.results[i].item_id;
                result.year = lresults.results[i].year;
                results.Add(result);
            }
            for (i = 0; i < lresults.abc_calc_agg.Length; i++)
            {
                abc_calc_agg=new abc_calc_agg();
                abc_calc_agg.result_row_id = lresults.abc_calc_agg[i].result_row_id;
                abc_calc_agg.year = lresults.abc_calc_agg[i].year;
                abc_calc_agg.contributor_id = lresults.abc_calc_agg[i].contributor_id;
                abc_calc_agg.value_1 = lresults.abc_calc_agg[i].value_1;
                abc_calc_agg.value_2 = lresults.abc_calc_agg[i].value_2;
                abc_calc_agg.value_3 = lresults.abc_calc_agg[i].value_3;
                abc_calc_agg.value_4 = lresults.abc_calc_agg[i].value_4;
                abc_calc_agg.value_5 = lresults.abc_calc_agg[i].value_5;
                abc_calc_agg.value_6 = lresults.abc_calc_agg[i].value_6;
                abc_calc_agg.value_7 = lresults.abc_calc_agg[i].value_7;
                abc_calc_agg.value_8 = lresults.abc_calc_agg[i].value_8;
                abc_calc_agg.entity_id = lresults.abc_calc_agg[i].entity_id;
                _abc_calc_agg.Add(abc_calc_agg);
            }
         
            for (i = 0; i < lresults.abc_calc_agg_growths.Length; i++)
            {
                abc_calc_agg = new abc_calc_agg();
                abc_calc_agg.result_row_id = lresults.abc_calc_agg_growths[i].result_row_id;
                abc_calc_agg.year = lresults.abc_calc_agg_growths[i].year;
                abc_calc_agg.contributor_id = lresults.abc_calc_agg_growths[i].contributor_id;
                abc_calc_agg.value_1 = lresults.abc_calc_agg_growths[i].value_1;
                abc_calc_agg.value_2 = lresults.abc_calc_agg_growths[i].value_2;
                abc_calc_agg.value_3 = lresults.abc_calc_agg_growths[i].value_3;
                abc_calc_agg.value_4 = lresults.abc_calc_agg_growths[i].value_4;
                abc_calc_agg.value_5 = lresults.abc_calc_agg_growths[i].value_5;
                abc_calc_agg.value_6 = lresults.abc_calc_agg_growths[i].value_6;
                abc_calc_agg.value_7 = lresults.abc_calc_agg_growths[i].value_7;
                abc_calc_agg.value_8 = lresults.abc_calc_agg_growths[i].value_8;
                abc_calc_agg.entity_id = lresults.abc_calc_agg_growths[i].entity_id;
                abc_calc_agg.include_in_growthr = lresults.abc_calc_agg_growths[i].include_in_growthr;
                abc_calc_agg.include_in_growthl = lresults.abc_calc_agg_growths[i].include_in_growthl;
                abc_calc_agg.num_years = lresults.abc_calc_agg_growths[i].num_years;
                _abc_calc_agg_growths.Add(abc_calc_agg);
            }
            for (i = 0; i < lresults.abc_calc_agg_cc.Length; i++)
            {
                abc_calc_agg = new abc_calc_agg();
                abc_calc_agg.result_row_id = lresults.abc_calc_agg_cc[i].result_row_id;
                abc_calc_agg.year = lresults.abc_calc_agg_cc[i].year;
                abc_calc_agg.contributor_id = lresults.abc_calc_agg_cc[i].contributor_id;
                abc_calc_agg.value_1 = lresults.abc_calc_agg_cc[i].value_1;
                abc_calc_agg.value_2 = lresults.abc_calc_agg_cc[i].value_2;
                abc_calc_agg.value_3 = lresults.abc_calc_agg_cc[i].value_3;
                abc_calc_agg.value_4 = lresults.abc_calc_agg_cc[i].value_4;
                abc_calc_agg.value_5 = lresults.abc_calc_agg_cc[i].value_5;
                abc_calc_agg.value_6 = lresults.abc_calc_agg_cc[i].value_6;
                abc_calc_agg.value_7 = lresults.abc_calc_agg_cc[i].value_7;
                abc_calc_agg.value_8 = lresults.abc_calc_agg_cc[i].value_8;
                abc_calc_agg.entity_id = lresults.abc_calc_agg_cc[i].entity_id;
                abc_calc_agg.contributor_1_id = lresults.abc_calc_agg_cc[i].contributor_1_id;
                abc_calc_agg.contributor_2_id = lresults.abc_calc_agg_cc[i].contributor_2_id;
                _abc_calc_agg_cc.Add(abc_calc_agg);
            }

            set_results();
        }


        void set_results()
        {
            uxresults.Nodes.Clear();
            try
            {
                if (results.Count != 0)
                {
                    uxresults.BeginUnboundLoad();
                    uxresults.Nodes.Clear();
                    int i, j;
                    xtraTabControl1.TabPages[0].Text = "Results (" + results.Count.ToString() + ")";
                    for (i = 0; i < results.Count; i++)
                    {
                        if (results[i].contributor_id == _universes.luniverses[uxuniverse.SelectedIndex].contributors[uxcont.SelectedIndex].contributor_id)
                        {
                            uxresults.Nodes.Add(results[i].value);
                            for (j = 0; j < _universes.luniverses[uxuniverse.SelectedIndex].metrics.Count; j++)
                            {
                                if (_universes.luniverses[uxuniverse.SelectedIndex].metrics[j].metric_id == results[i].item_id)
                                {
                                    uxresults.Nodes[i].SetValue(0, _universes.luniverses[uxuniverse.SelectedIndex].metrics[j].metric_name);
                                    uxresults.Nodes[i].Tag = _universes.luniverses[uxuniverse.SelectedIndex].metrics[j].metric_id;
                                    break;
                                }
                            }
                        }
                        uxresults.Nodes[i].SetValue(1, results[i].year);
                        uxresults.Nodes[i].SetValue(2, results[i].value);
                    }

                }
            }
            catch (Exception err)
            {
                //MessageBox.Show(err.ToString());
            }
            finally{
                    uxresults.EndUnboundLoad();
            }
        }

        public bool load_view(universes universes, Cbc_am_aggs_preview controller) 
        {
            _contoller = controller;
            _universes = universes;
            int i;
            for (i = 0; i < universes.luniverses.Count;i++ )
            {
                uxuniverse.Properties.Items.Add(universes.luniverses[i].universe_name);
            }
            uxstage.Properties.Items.Add("Publish");
            uxstage.Enabled = false;
            uxstage.SelectedIndex = 0;

            if (_universes.luniverses.Count==1)
            {
                uxuniverse.SelectedIndex = 0;
                uxuniverse.Enabled = false;
            }
            return true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

       
       

        private void bcancel_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void uxuniverse_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            uxtargetclass.Properties.Items.Clear();
            uxentity.Properties.Items.Clear();
            uxdual.Properties.Items.Clear();
            uxcont.Properties.Items.Clear();
            uxexch.Properties.Items.Clear();
            uxtype.Properties.Items.Clear();
            uxtargetclass.SelectedIndex = -1;
            uxentity.SelectedIndex = -1;
            uxdual.SelectedIndex = -1;
            uxcont.SelectedIndex = -1;
            uxexch.SelectedIndex = -1;
            uxtype.SelectedIndex = -1;
            if (uxuniverse.SelectedIndex > -1)
            {
                string s;
                int i;
              
                if (_universes.luniverses[uxuniverse.SelectedIndex].exch_rate_method == 0 || _universes.luniverses[uxuniverse.SelectedIndex].exch_rate_method == 3)
                {
                    uxexch.Properties.Items.Add("Current");
                }
                if (_universes.luniverses[uxuniverse.SelectedIndex].exch_rate_method == 1)
                {
                    uxexch.Properties.Items.Add("Period End");
                }
                if (_universes.luniverses[uxuniverse.SelectedIndex].exch_rate_method == 2 || _universes.luniverses[uxuniverse.SelectedIndex].exch_rate_method == 3)
                {
                    uxexch.Properties.Items.Add("Period Average");
                }
                uxexch.SelectedIndex = 0;
                if (uxexch.Properties.Items.Count == 1)
                    uxexch.Enabled = false;
                else
                    uxexch.Enabled = true;

                for (i = 0; i < _universes.luniverses[uxuniverse.SelectedIndex].calc_types.Count; i++)
                {
                    uxtype.Properties.Items.Add(_universes.luniverses[uxuniverse.SelectedIndex].calc_types[i]);
                }
                if (uxtype.Properties.Items.Count > 0)
                  uxtype.SelectedIndex = 0;
                if (uxtype.Properties.Items.Count == 1)
                    uxtype.Enabled = false;
                else
                    uxtype.Enabled = true;


                for (i = 0; i < _universes.luniverses[uxuniverse.SelectedIndex].target_classs.Count; i++)
                {
                    s = _universes.luniverses[uxuniverse.SelectedIndex].target_classs[i].class_name;
                    if (_universes.luniverses[uxuniverse.SelectedIndex].target_classs[i].dual_class_id > 0)
                    {
                        s = s + " & " + _universes.luniverses[uxuniverse.SelectedIndex].target_classs[i].dual_class_name;
                    }
                    uxtargetclass.Properties.Items.Add(s);

                    if (uxtargetclass.Properties.Items.Count == 1)
                    {
                        uxtargetclass.SelectedIndex = 0;
                      
                    }
                    else
                        uxtargetclass.Enabled = true;
                }
                for (i = 0; i < _universes.luniverses[uxuniverse.SelectedIndex].contributors.Count; i++)
                {
                    uxcont.Properties.Items.Add(_universes.luniverses[uxuniverse.SelectedIndex].contributors[i].contributor_name);
                    if (uxcont.Properties.Items.Count == 1)
                    {
                        uxcont.SelectedIndex = 0;
                        uxcont.Enabled = false;
                    }
                    else
                    {
                        uxcont.SelectedIndex = 0;
                        uxcont.Enabled = true;
                    }
                }

            }
        }

        private void uxtargetclass_SelectedIndexChanged(object sender, EventArgs e)
        {
            uxentity.Enabled = false;
            uxdual.Enabled = false;
            if (uxtargetclass.SelectedIndex != -1)
            {
                check_can_run();
                uxentity.Enabled = true;
                int i;
                 uxentity.Properties.Items.Clear();
                 uxdual.Properties.Items.Clear();
                for (i=0; i < _universes.entities.Count; i++)
                {
                    if (_universes.entities[i].class_id == _universes.luniverses[uxuniverse.SelectedIndex].target_classs[uxtargetclass.SelectedIndex].class_id)
                    {
                        uxentity.Properties.Items.Add (_universes.entities[i].name);
                        if (uxtargetclass.Text == "Aggregation Universe")
                        {
                            uxentity.Text = uxuniverse.Text;
                            uxentity.Enabled = false;
                            uxtargetclass.Enabled = false;
                            check_can_run();
                        }
                    }
                    if (_universes.entities[i].class_id == _universes.luniverses[uxuniverse.SelectedIndex].target_classs[uxtargetclass.SelectedIndex].dual_class_id)
                    {
                        uxdual.Properties.Items.Add(_universes.entities[i].name);
                        uxdual.Enabled = true;
                    }
                }
            }
        }

        private void bok_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                xtraTabControl1.Enabled = false;
              
                long entity_id = 0;
                long dual_entity_id = 0;
                int i;
                for (i = 0; i < _universes.entities.Count - 1; i++)
                {
                    if (_universes.entities[i].name == uxentity.SelectedItem.ToString() && _universes.entities[i].class_id == _universes.luniverses[uxuniverse.SelectedIndex].target_classs[uxtargetclass.SelectedIndex].class_id)
                    {
                        entity_id = _universes.entities[i].entity_id;

                    }
                    if (uxdual.SelectedIndex > -1)
                    {
                        if (_universes.entities[i].name == uxdual.SelectedItem.ToString() && _universes.entities[i].class_id == _universes.luniverses[uxuniverse.SelectedIndex].target_classs[uxtargetclass.SelectedIndex].dual_class_id)
                        {
                            dual_entity_id = _universes.entities[i].entity_id;

                        }
                    }

                }
                int exch_type=0;
                if (uxexch.Text == "Period")
                     exch_type=1;
                else if (uxexch.Text == "Period Average")
                    exch_type=2;
              

                //_contoller.run(_universes.luniverses[uxuniverse.SelectedIndex]._universe_id, entity_id, dual_entity_id, exch_type, uxtype.Text);
                xtraTabControl1.Enabled = true;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void check_can_run()
        {
            bok.Enabled = false;
            if (uxuniverse.SelectedIndex != -1 && uxtargetclass.SelectedIndex != -1 && uxentity.SelectedIndex != -1)
            {
                if (uxdual.Enabled==true)
                {
                    if (uxdual.SelectedIndex != -1)
                        bok.Enabled=true;
                }
                else
                    bok.Enabled=true;
            }
        }
        private void uxmetric_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check_can_run();
        }

        private void uxentity_SelectedIndexChanged(object sender, EventArgs e)
        {
            check_can_run();
        }

        private void uxdual_SelectedIndexChanged(object sender, EventArgs e)
        {
            check_can_run();
        }

        private void uxcont_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (uxcont.SelectedIndex > -1)
                    set_results();
            }
            catch
            {

            }
        }

        private void uxyear_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check_can_run();
        }

        private void uxresults_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            try
            {
                uxconts.Visible = false;
                lconstdesc.Text = "";
                int year;
                long metric_id;
                string value;
                year = (int)uxresults.FocusedNode.GetValue(1);
                metric_id = (long)uxresults.FocusedNode.Tag;
                value = uxresults.FocusedNode.GetValue(2).ToString();
                // load breakdown of this metric
                lconstdesc.Text = uxresults.FocusedNode.GetValue(0).ToString() + ": " + year + ": " + uxcont.Text + ": " + value;
                int i;
                for (i=0; i <  _universes.luniverses[uxuniverse.SelectedIndex].metrics.Count-1;i++)
                {
                    if (_universes.luniverses[uxuniverse.SelectedIndex].metrics[i].metric_id==metric_id)
                    {
                        lformula.Text = _universes.luniverses[uxuniverse.SelectedIndex].metrics[i].formula.Trim();
                        break;
                    }
                }
               
                uxconts.Visible = true;
                uxconts.BeginUnboundLoad();
                uxconts.Nodes.Clear();
                uxconts.Columns[1].Visible = false;
                uxconts.Columns[2].Visible = false;
                uxconts.Columns[3].Visible = false;
                uxconts.Columns[4].Visible = false;
                uxconts.Columns[5].Visible = false;
                uxconts.Columns[6].Visible = false;
                uxconts.Columns[7].Visible = false;
                uxconts.Columns[8].Visible = false;
                uxconts.Columns[9].Visible = false;
                uxconts.Columns[10].Visible = false;
                uxconts.Columns[11].Visible = false;
                uxconts.Columns[12].Visible = false;
                uxconts.Columns[13].Visible = false;
                uxconts.Columns[14].Visible = false;
                uxconts.Columns[15].Visible = false;
                uxconts.Columns[16].Visible = false;



                for (i=0; i < _universes.luniverses[uxuniverse.SelectedIndex].metrics.Count; i++)
                {
                    if ( _universes.luniverses[uxuniverse.SelectedIndex].metrics[i].metric_id== metric_id )
                    {
                        if (_universes.luniverses[uxuniverse.SelectedIndex].metrics[i].num_years == 0 && _universes.luniverses[uxuniverse.SelectedIndex].metrics[i].contributor2 == 0)
                        {
                            load_conts(_abc_calc_agg, metric_id, year);
                        }
                        else   if (_universes.luniverses[uxuniverse.SelectedIndex].metrics[i].num_years >0)
                        {
                            load_conts_growth(_abc_calc_agg_growths, metric_id, year);
                        }
                        else
                        {
                            load_conts_cc(_abc_calc_agg_cc, metric_id, year);
                        }
                    }
                }
                uxconts.EndUnboundLoad();
            
            }
            catch (Exception err)
            {
                //MessageBox.Show(err.ToString());
            }
        }

        void load_conts(List <abc_calc_agg> stage, long metric_id, int year)
        {
          int k,l;
          int nc=0;

            for (k = 0; k < stage.Count; k++)
            {
                if (stage[k].result_row_id == metric_id && stage[k].year == year && stage[k].contributor_id == _universes.luniverses[uxuniverse.SelectedIndex].contributors[uxcont.SelectedIndex].contributor_id)
                {

                    for (l = 0; l < _universes.entities.Count; l++)
                    {
                        if (_universes.entities[l].entity_id == stage[k].entity_id)
                        {
                            uxconts.Nodes.Add(_universes.entities[l].name + " [" + stage[k].entity_id.ToString() + "]");
                            uxconts.Nodes[nc].SetValue(0, _universes.entities[l].name + " [" + stage[k].entity_id.ToString() + "]");
                            break;
                        }
                    }

                    if (stage[k].value_1 != null)
                    {
                        uxconts.Nodes[nc].SetValue(1, stage[k].value_1);
                        uxconts.Columns[1].Visible = true;
                        uxconts.Columns[1].VisibleIndex = 1;
                    }
                    if (stage[k].value_2 != null)
                    {
                        uxconts.Nodes[nc].SetValue(3, stage[k].value_2);
                        uxconts.Columns[3].Visible = true;
                        uxconts.Columns[3].VisibleIndex = 2;
                    }
                    if (stage[k].value_3 != null)
                    {
                        uxconts.Nodes[nc].SetValue(5, stage[k].value_3);
                        uxconts.Columns[5].Visible = true;
                        uxconts.Columns[5].VisibleIndex = 3;
                    }
                    if (stage[k].value_4 != null)
                    {
                        uxconts.Nodes[nc].SetValue(7, stage[k].value_4);
                        uxconts.Columns[7].Visible = true;
                        uxconts.Columns[7].VisibleIndex = 4;
                    }
                    if (stage[k].value_5 != null)
                    {
                        uxconts.Nodes[nc].SetValue(9, stage[k].value_5);
                        uxconts.Columns[9].Visible = true;
                        uxconts.Columns[9].VisibleIndex = 5;
                    }
                    if (stage[k].value_6 != null)
                    {
                        uxconts.Nodes[nc].SetValue(11, stage[k].value_6);
                        uxconts.Columns[11].Visible = true;
                        uxconts.Columns[11].VisibleIndex = 6;
                    }
                    if (stage[k].value_7 != null)
                    {
                        uxconts.Nodes[nc].SetValue(13, stage[k].value_7);
                        uxconts.Columns[13].Visible = true;
                        uxconts.Columns[13].VisibleIndex = 7;
                    }
                    if (stage[k].value_8 != null)
                    {
                        uxconts.Nodes[nc].SetValue(15, stage[k].value_8);
                        uxconts.Columns[15].Visible = true;
                        uxconts.Columns[15].VisibleIndex = 8;
                    }
                    nc = nc + 1;
                }
            }
            xtraTabPage2.Text = "Constituents (" + nc.ToString() + ")";
        }
 
    void load_conts_growth(List <abc_calc_agg> stage, long metric_id, int year)
        {
          
          int k,l;
          int nc=0;
          int lyear=year;
         
         
          for (k = 0; k < stage.Count; k++)
            {
                if (stage[k].include_in_growthr==true && stage[k].result_row_id == metric_id && stage[k].year == year && stage[k].contributor_id == _universes.luniverses[uxuniverse.SelectedIndex].contributors[uxcont.SelectedIndex].contributor_id)
                {
                    lyear = year - stage[k].num_years;
                    for (l = 0; l < _universes.entities.Count; l++)
                    {
                        if (_universes.entities[l].entity_id == stage[k].entity_id)
                        {
                            uxconts.Nodes.Add(_universes.entities[l].name + " [" + stage[k].entity_id.ToString() + "]");
                            uxconts.Nodes[nc].SetValue(0, _universes.entities[l].name + " [" + stage[k].entity_id.ToString() + "]");
                            uxconts.Nodes[nc].Tag = stage[k].entity_id;
                            break;
                        }
                    }

                    if (stage[k].value_1 != null)
                    {
                        uxconts.Nodes[nc].SetValue(1, stage[k].value_1);
                        uxconts.Columns[1].Visible = true;
                        uxconts.Columns[1].VisibleIndex = 1;
                    }
                    if (stage[k].value_2 != null)
                    {
                        uxconts.Nodes[nc].SetValue(3, stage[k].value_2);
                        uxconts.Columns[3].Visible = true;
                        uxconts.Columns[3].VisibleIndex = 3;
                    }
                    if (stage[k].value_3 != null)
                    {
                        uxconts.Nodes[nc].SetValue(5, stage[k].value_3);
                        uxconts.Columns[5].Visible = true;
                        uxconts.Columns[5].VisibleIndex = 5;
                    }
                    if (stage[k].value_4 != null)
                    {
                        uxconts.Nodes[nc].SetValue(7, stage[k].value_4);
                        uxconts.Columns[7].Visible = true;
                        uxconts.Columns[7].VisibleIndex = 7;
                    }
                    if (stage[k].value_5 != null)
                    {
                        uxconts.Nodes[nc].SetValue(9, stage[k].value_5);
                        uxconts.Columns[9].Visible = true;
                        uxconts.Columns[9].VisibleIndex = 9;
                    }
                    if (stage[k].value_6 != null)
                    {
                        uxconts.Nodes[nc].SetValue(11, stage[k].value_6);
                        uxconts.Columns[11].Visible = true;
                        uxconts.Columns[11].VisibleIndex = 11;
                    }
                    if (stage[k].value_7 != null)
                    {
                        uxconts.Nodes[nc].SetValue(13, stage[k].value_7);
                        uxconts.Columns[13].Visible = true;
                        uxconts.Columns[13].VisibleIndex = 13;
                    }
                    if (stage[k].value_8 != null)
                    {
                        uxconts.Nodes[nc].SetValue(15, stage[k].value_8);
                        uxconts.Columns[15].Visible = true;
                        uxconts.Columns[15].VisibleIndex = 15;
                    }
                    nc = nc + 1;
                }
            }
       
          nc = 0;
         
          for (k = 0; k < stage.Count; k++)
          {
              if (stage[k].include_in_growthl == true && stage[k].result_row_id == metric_id && stage[k].year == lyear && stage[k].contributor_id == _universes.luniverses[uxuniverse.SelectedIndex].contributors[uxcont.SelectedIndex].contributor_id)
              {
                  for (nc = 0; nc < uxconts.Nodes.Count; nc++ )
                      if (uxconts.Nodes[nc].Tag.ToString() == stage[k].entity_id.ToString())
                      {

                          if (stage[k].value_1 != null)
                          {
                              uxconts.Nodes[nc].SetValue(2, stage[k].value_1);
                              uxconts.Columns[2].Visible = true;
                              uxconts.Columns[2].VisibleIndex = 2;
                              uxconts.Columns[2].Caption = "value_1_" + lyear.ToString();
                          }
                          if (stage[k].value_2 != null)
                          {
                              uxconts.Nodes[nc].SetValue(4, stage[k].value_2);
                              uxconts.Columns[4].Visible = true;
                              uxconts.Columns[4].VisibleIndex = 4;
                              uxconts.Columns[4].Caption = "value_2_" + lyear.ToString();
                          }
                          if (stage[k].value_3 != null)
                          {
                              uxconts.Nodes[nc].SetValue(6, stage[k].value_3);
                              uxconts.Columns[6].Visible = true;
                              uxconts.Columns[6].VisibleIndex = 6;
                              uxconts.Columns[6].Caption = "value_3_" + lyear.ToString();
                          }
                          if (stage[k].value_4 != null)
                          {
                              uxconts.Nodes[nc].SetValue(8, stage[k].value_4);
                              uxconts.Columns[8].Visible = true;
                              uxconts.Columns[8].VisibleIndex = 8;
                              uxconts.Columns[8].Caption = "value_4_" + lyear.ToString();
                          }
                          if (stage[k].value_5 != null)
                          {
                              uxconts.Nodes[nc].SetValue(10, stage[k].value_5);
                              uxconts.Columns[10].Visible = true;
                              uxconts.Columns[10].VisibleIndex = 10;
                              uxconts.Columns[10].Caption = "value_5_" + lyear.ToString();
                          }
                          if (stage[k].value_6 != null)
                          {
                              uxconts.Nodes[nc].SetValue(12, stage[k].value_6);
                              uxconts.Columns[12].Visible = true;
                              uxconts.Columns[12].VisibleIndex = 12;
                              uxconts.Columns[12].Caption = "value_6_" + lyear.ToString();
                          }
                          if (stage[k].value_7 != null)
                          {
                              uxconts.Nodes[nc].SetValue(14, stage[k].value_7);
                              uxconts.Columns[14].Visible = true;
                              uxconts.Columns[14].VisibleIndex = 14;
                              uxconts.Columns[14].Caption = "value_7_" + lyear.ToString();
                          }
                          if (stage[k].value_8 != null)
                          {
                              uxconts.Nodes[nc].SetValue(16, stage[k].value_8);
                              uxconts.Columns[16].Visible = true;
                              uxconts.Columns[16].VisibleIndex = 16;
                              uxconts.Columns[16].Caption = "value_8_" + lyear.ToString();
                          }
                      }
              }
          }
            xtraTabPage2.Text = "Constituents (" + nc.ToString() + ")";
        }

    void load_conts_cc(List <abc_calc_agg> stage, long metric_id, int year)
        {
          
          int k,l;
          int nc=0;
        
          long lcont = 1;
         
          for (k = 0; k < stage.Count; k++)
            {
                if (stage[k].result_row_id == metric_id && stage[k].year == year && stage[k].contributor_id == _universes.luniverses[uxuniverse.SelectedIndex].contributors[uxcont.SelectedIndex].contributor_id)
                {
                    lcont = stage[k].contributor_2_id;
                    for (l = 0; l < _universes.entities.Count; l++)
                    {
                        if (_universes.entities[l].entity_id == stage[k].entity_id)
                        {
                            uxconts.Nodes.Add(_universes.entities[l].name + " [" + stage[k].entity_id.ToString() + "]");
                            uxconts.Nodes[nc].SetValue(0, _universes.entities[l].name + " [" + stage[k].entity_id.ToString() + "]");
                            uxconts.Nodes[nc].Tag = stage[k].entity_id;
                            break;
                        }
                    }

                    if (stage[k].value_1 != null)
                    {
                        uxconts.Nodes[nc].SetValue(1, stage[k].value_1);
                        uxconts.Columns[1].Visible = true;
                        uxconts.Columns[1].VisibleIndex = 1;
                    }
                    if (stage[k].value_2 != null)
                    {
                        uxconts.Nodes[nc].SetValue(3, stage[k].value_2);
                        uxconts.Columns[3].Visible = true;
                        uxconts.Columns[3].VisibleIndex = 3;
                    }
                    if (stage[k].value_3 != null)
                    {
                        uxconts.Nodes[nc].SetValue(5, stage[k].value_3);
                        uxconts.Columns[5].Visible = true;
                        uxconts.Columns[5].VisibleIndex = 5;
                    }
                    if (stage[k].value_4 != null)
                    {
                        uxconts.Nodes[nc].SetValue(7, stage[k].value_4);
                        uxconts.Columns[7].Visible = true;
                        uxconts.Columns[7].VisibleIndex = 7;
                    }
                    if (stage[k].value_5 != null)
                    {
                        uxconts.Nodes[nc].SetValue(9, stage[k].value_5);
                        uxconts.Columns[9].Visible = true;
                        uxconts.Columns[9].VisibleIndex = 9;
                    }
                    if (stage[k].value_6 != null)
                    {
                        uxconts.Nodes[nc].SetValue(11, stage[k].value_6);
                        uxconts.Columns[11].Visible = true;
                        uxconts.Columns[11].VisibleIndex = 11;
                    }
                    if (stage[k].value_7 != null)
                    {
                        uxconts.Nodes[nc].SetValue(13, stage[k].value_7);
                        uxconts.Columns[13].Visible = true;
                        uxconts.Columns[13].VisibleIndex = 13;
                    }
                    if (stage[k].value_8 != null)
                    {
                        uxconts.Nodes[nc].SetValue(15, stage[k].value_8);
                        uxconts.Columns[15].Visible = true;
                        uxconts.Columns[15].VisibleIndex = 15;
                    }
                    nc = nc + 1;
                }
            }
       
          nc = 0;
         
          for (k = 0; k < stage.Count; k++)
          {
              if (stage[k].contributor_2_id==lcont  && stage[k].result_row_id == metric_id && stage[k].year == year && stage[k].contributor_id == lcont)
              {
                  for (nc = 0; nc < uxconts.Nodes.Count; nc++ )
                      if (uxconts.Nodes[nc].Tag.ToString() == stage[k].entity_id.ToString())
                      {

                          if (stage[k].value_1 != null)
                          {
                              uxconts.Nodes[nc].SetValue(2, stage[k].value_1);
                              uxconts.Columns[2].Visible = true;
                              uxconts.Columns[2].VisibleIndex = 2;
                              uxconts.Columns[2].Caption = "value_1_c_" + lcont.ToString();
                          }
                          if (stage[k].value_2 != null)
                          {
                              uxconts.Nodes[nc].SetValue(4, stage[k].value_2);
                              uxconts.Columns[4].Visible = true;
                              uxconts.Columns[4].VisibleIndex = 4;

                              uxconts.Columns[4].Caption = "value_2_c_" + lcont.ToString();
                          }
                          if (stage[k].value_3 != null)
                          {
                              uxconts.Nodes[nc].SetValue(6, stage[k].value_3);
                              uxconts.Columns[6].Visible = true;
                              uxconts.Columns[6].VisibleIndex = 6;
                              uxconts.Columns[6].Caption = "value_3_c_" + lcont.ToString();
                          }
                          if (stage[k].value_4 != null)
                          {
                              uxconts.Nodes[nc].SetValue(8, stage[k].value_4);
                              uxconts.Columns[8].Visible = true;
                              uxconts.Columns[8].VisibleIndex = 8;
                              uxconts.Columns[8].Caption = "value_4_c_" + lcont.ToString();
                          }
                          if (stage[k].value_5 != null)
                          {
                              uxconts.Nodes[nc].SetValue(10, stage[k].value_5);
                              uxconts.Columns[10].Visible = true;
                              uxconts.Columns[10].VisibleIndex = 10;
                              uxconts.Columns[10].Caption = "value_5_c_" + lcont.ToString();
                          }
                          if (stage[k].value_6 != null)
                          {
                              uxconts.Nodes[nc].SetValue(12, stage[k].value_6);
                              uxconts.Columns[12].Visible = true;
                              uxconts.Columns[12].VisibleIndex = 12;
                              uxconts.Columns[12].Caption = "value_6_c_" + lcont.ToString();
                          }
                          if (stage[k].value_7 != null)
                          {
                              uxconts.Nodes[nc].SetValue(14, stage[k].value_7);
                              uxconts.Columns[14].Visible = true;
                              uxconts.Columns[14].VisibleIndex = 14;
                              uxconts.Columns[14].Caption = "value_7_c_" + lcont.ToString();
                          }
                          if (stage[k].value_8 != null)
                          {
                              uxconts.Nodes[nc].SetValue(16, stage[k].value_8);
                              uxconts.Columns[16].Visible = true;
                              uxconts.Columns[16].VisibleIndex = 16;
                              uxconts.Columns[16].Caption = "value_8_c_" + lcont.ToString();
                          }
                      }
              }
          }
            xtraTabPage2.Text = "Constituents (" + nc.ToString() + ")";
        }

    private void uxexch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (uxcont.SelectedIndex > -1)
                set_results();
        }
        catch
        {

        }
    }
    }
    public class Cbc_am_aggs_preview
    {
        bc_cs_central_settings bcs = new bc_cs_central_settings(true);
        Ibc_am_aggs_preview _view;

      

        public bool load_data(long universe_id=0)
        {
            try
            {
               
                bc_cs_central_settings bcs= new bc_cs_central_settings(true);
                bc_am_aggs_preview view = new bc_am_aggs_preview();
                _view = view;
                universes universes = new universes();
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                certificate.user_id = "Aggregation Preview";

                universes.db_read(ref certificate, universe_id);
                if (_view.load_view(universes,this) == true)
                {
                    view.ShowDialog();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message.ToString());
                return false;
            }
        }

        public bool run(long universe_id, long target_entity_id, long dual_entity_id, int exch_rate_method, string type, bool ic)
        {
            try
            {
                bc_cs_security.certificate certificate = new bc_cs_security.certificate();
                certificate.user_id = "BluecurveAggregationPreview";
                bc_am_aggs_service_based raggs = new bc_am_aggs_service_based();
                bc_cs_sql bsql = new bc_cs_sql();
                bc_cs_db_services gdb = new bc_cs_db_services();

                object res = null;
                Array ares;
                int audit_id;
                DateTime audit_date;


                if (bsql.exec_sql("exec dbo.bc_core_aggs_services_preview_get_audit_details", bc_cs_sql.SQL_TYPE.NO_TIMEOUT, ref res) == false)
                    return false;


                if (res != null)
                {
                    ares = (Array)res;
                    audit_id = (int)ares.GetValue(0, 0);
                    audit_date = (DateTime)ares.GetValue(1, 0);

                    BasicHttpBinding binding = new BasicHttpBinding();
                    binding.MaxReceivedMessageSize = 2147483647;
                    
                    EndpointAddress ea = new EndpointAddress(bc_cs_central_settings.aggregation_system_url);

                    BlueCurve.Aggregations.AM.ServiceReference1.BCIISServicesClient s = new BlueCurve.Aggregations.AM.ServiceReference1.BCIISServicesClient(binding, ea);
                    s.Endpoint.Binding.SendTimeout = TimeSpan.FromMilliseconds(9999999);

                    string err;
                    BlueCurve.Aggregations.AM.ServiceReference1.agg_results results = new BlueCurve.Aggregations.AM.ServiceReference1.agg_results();
                    try
                    {
                        results = s.AggregateUniverseDebug(0, universe_id, audit_id, audit_date, target_entity_id, dual_entity_id, exch_rate_method, type, ic);
                    
                        if (results.error != "")
                        {
                             bc_cs_message msg = new bc_cs_message("Blue Curve", "Error Aggregating Universe: " + results.error, bc_cs_message.MESSAGE, false, false, "Yes", "No", true);
                        }
                        else if (results.results.Length==0)
                        {
                             bc_cs_message msg = new bc_cs_message("Blue Curve", "No Results for Target", bc_cs_message.MESSAGE, false, false, "Yes", "No", true);
          
                        }
                        else
                        {
                           _view.load_results(results);
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        err = ex.Message;
                        MessageBox.Show(err);
                    }
          
                }
                return true;
            }
            //} 
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
    public interface Ibc_am_aggs_preview
    {
        bool load_view(universes universes, Cbc_am_aggs_preview controller);
        void load_results(BlueCurve.Aggregations.AM.ServiceReference1.agg_results results);
    }
    
    public class universes
    {
        public List<universe> luniverses = new List<universe>();
        public List<entity> entities = new List<entity>();
        public void db_read(ref bc_cs_security.certificate certificate, long universe_id)
        {
            try
            {
                bc_cs_security.certificate certifcate = new bc_cs_security.certificate();

                bc_cs_db_services gdb = new bc_cs_db_services();
                //string sql;
                object res = null;
                Array ares;

                universe universe;
                entity entity;
                int i;
                bc_cs_sql bsql = new bc_cs_sql();


                if (bsql.exec_sql("exec dbo.bc_core_aggs_services_preview_get_entities", bc_cs_sql.SQL_TYPE.NO_TIMEOUT, ref res) == false)
                    return;

                if (res != null)
                {
                    ares = (Array)res;
                    for (i = 0; i <= ares.GetUpperBound(1); i++)
                    {
                        entity = new entity();
                        entity.entity_id = (long)ares.GetValue(0, i);
                        entity.name = (string)ares.GetValue(1, i);
                        entity.class_id = (long)ares.GetValue(2, i);

                        entities.Add(entity);
                    }
                }

                if (bsql.exec_sql("exec dbo.bc_core_aggs_services_preview_get_universes", bc_cs_sql.SQL_TYPE.NO_TIMEOUT, ref res) == false)
                    return;

                if (res != null)
                {
                    ares = (Array)res;
                    for (i = 0; i <= ares.GetUpperBound(1); i++)
                    {
                        universe = new universe();
                        universe.universe_id = (long)ares.GetValue(0, i);
                        universe.universe_name = (string)ares.GetValue(1, i);
                        if (universe_id == 0 || (universe_id == universe.universe_id))
                        {
                            universe.db_read(ref certificate);
                            luniverses.Add(universe);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("db_read: " + err.Message.ToString());
            }
        }
    }
    public class universe
    {
        public long _universe_id;
        public string _universe_name;
        public List<target_class> target_classs = new List<target_class>();
        public List<int> years = new List<int>();
        public List<contributor> contributors = new List<contributor>();
        public List<metric> metrics = new List<metric>();
        public int  _exch_rate_method;
        public List<string> calc_types= new List<string>();

        public int exch_rate_method
        {
            get { return _exch_rate_method; }
            set { _exch_rate_method = value; }
        }


        public long universe_id
        {
            get { return _universe_id; }
            set { _universe_id = value; }
        }

        public string universe_name
        {
            get { return _universe_name; }
            set { _universe_name = value; }
        }

        public void db_read(ref bc_cs_security.certificate certificate)
        {
            bc_cs_db_services gdb = new bc_cs_db_services();
            target_class target_class;
            contributor contributor;
            metric metric;
            //string sql;
            object res=null;
            Array ares;
            bc_cs_sql bsql = new bc_cs_sql();
            int i;
            if (bsql.exec_sql("exec dbo.bc_core_aggs_services_preview_get_universe_target " + _universe_id.ToString(), bc_cs_sql.SQL_TYPE.NO_TIMEOUT, ref res) == false)
                return ;
           
            ares = (Array)res;
            for (i = 0; i <= ares.GetUpperBound(1); i++)
            {
                target_class = new target_class();
                target_class.class_id = (long)ares.GetValue(0, i);
                target_class.class_name = (string)ares.GetValue(1, i);
                target_class.dual_class_id = (long)ares.GetValue(2, i);
                target_class.dual_class_name = (string)ares.GetValue(3, i);
                target_classs.Add(target_class);
            }
           
          

            if (bsql.exec_sql("exec dbo.bc_core_aggs_services_preview_get_contributors " + _universe_id.ToString(), bc_cs_sql.SQL_TYPE.NO_TIMEOUT, ref res) == false)
                return;

            ares = (Array)res;
            for (i = 0; i <= ares.GetUpperBound(1); i++)
            {
                contributor = new contributor();
                contributor.contributor_id = (long)ares.GetValue(0, i);
                contributor.contributor_name = (string)ares.GetValue(1, i);
                contributors.Add(contributor);
            }
           
            if (bsql.exec_sql("exec dbo.bc_core_aggs_services_preview_get_years " + _universe_id.ToString(), bc_cs_sql.SQL_TYPE.NO_TIMEOUT, ref res) == false)
                return;
            ares = (Array)res;
            for (i = 0; i <= ares.GetUpperBound(1); i++)
            {
                years.Add( (int)ares.GetValue(0, i));
            }
           

            if (bsql.exec_sql("exec dbo.bc_core_aggs_services_preview_get_metrics " + _universe_id.ToString(), bc_cs_sql.SQL_TYPE.NO_TIMEOUT, ref res) == false)
                return;
            ares = (Array)res;
            for (i = 0; i <= ares.GetUpperBound(1); i++)
            {
              
                metric = new metric();
                metric.metric_id= (long)ares.GetValue(0, i);
                metric.metric_name = (string)ares.GetValue(1, i);
                metric.num_years = (int)ares.GetValue(2, i);
                metric.contributor2 = (int)ares.GetValue(3, i);
                metric.formula = (string)ares.GetValue(4, i);
                metrics.Add(metric);
            }
           
            if (bsql.exec_sql("exec dbo.bc_core_aggs_services_preview_get_exch_rate_method " + _universe_id.ToString(), bc_cs_sql.SQL_TYPE.NO_TIMEOUT, ref res) == false)
                return;
            ares = (Array)res;
            for (i = 0; i <= ares.GetUpperBound(1); i++)
            {
                exch_rate_method = (int)ares.GetValue(0, i);
            }

           
            if (bsql.exec_sql("exec dbo.bc_core_aggs_services_preview_get_calc_types " + _universe_id.ToString(), bc_cs_sql.SQL_TYPE.NO_TIMEOUT, ref res) == false)
                return;
            ares = (Array)res;
            for (i = 0; i <= ares.GetUpperBound(1); i++)
            {
               calc_types.Add ((String)ares.GetValue(0, i));
            }

        }
    }
    public class metric
    {
        long _metric_id;
        string _metric_name;
        int _num_years;
        int _contributor2;
        string _formula;
        public int num_years
        {
            get { return _num_years; }
            set { _num_years = value; }
        }


        public int contributor2
        {
            get { return _contributor2; }
            set { _contributor2 = value; }
        }

        public long metric_id
        {
            get { return _metric_id; }
            set { _metric_id = value; }
        }

        public string metric_name
        {
            get { return _metric_name; }
            set { _metric_name = value; }
        }
        public string formula
        {
            get { return _formula; }
            set { _formula = value; }
        }

    }
    public class contributor
    {
        long _contributor_id;
        string _contributor_name;
        public long contributor_id
        {
            get { return _contributor_id; }
            set { _contributor_id = value; }
        }

        public string contributor_name
        {
            get { return _contributor_name; }
            set { _contributor_name = value; }
        }

    }
    public class target_class
    {
        long _class_id;
        string _class_name;
        long _dual_class_id;
        string _dual_class_name;
        public long class_id
        {
            get { return _class_id; }
            set { _class_id = value; }
        }

        public string class_name
        {
            get { return _class_name; }
            set { _class_name = value; }
        }
        public long dual_class_id
        {
            get { return _dual_class_id; }
            set { _dual_class_id = value; }
        }

        public string dual_class_name
        {
            get { return _dual_class_name; }
            set { _dual_class_name = value; }
        }
    }
    public class entity
    {
        long _entity_id;
        long _class_id;
        string _name;
        public long entity_id
        {
            get { return _entity_id; }
            set { _entity_id = value; }
        }

        public long class_id
        {
            get { return _class_id; }
            set { _class_id = value; }
        }

        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}

