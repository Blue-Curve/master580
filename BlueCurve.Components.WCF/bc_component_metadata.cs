using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlueCurve.Core.OM;
using BlueCurve.Core.CS;
using bc_core_components_svc;
namespace bc_component_metadata
    
{
    public enum bc_component_mode {COMPONENT =1,TEMPLATE =2, DOCUMENT=3 }
    
    public class bc_component_metadata
    {
        bc_om_refresh_component _rc;
        bc_cs_security.certificate _certificate;
        long _template_id;
        long _comp_id;
        long _doc_id;
        string _locator;
        
        public bc_component_metadata(bc_om_refresh_component rc,long template_id,long comp_id,long doc_id, string locator, ref bc_cs_security.certificate certificate)
        {
            _certificate = certificate;
            _rc = rc;
            _template_id = template_id;
            _comp_id = comp_id;
            _doc_id = doc_id;
            _locator = locator;
        }
        public void set_metadata(bc_component_mode mode, List <bc_core_component_parameter> parameters)
        {
            try
            {
                object res;
                Array ares;
                bc_component_medata_db db = new bc_component_medata_db();
                res = db.get_metadata(_rc.type, ref _certificate);
                ares = (Array)res;
                if (ares.GetUpperBound(0) == 6)
                {
                    _rc.mode = Convert.ToInt32(ares.GetValue(0, 0).ToString());
                    _rc.mode_param1 = ares.GetValue(1, 0).ToString();
                    _rc.web_service_name = ares.GetValue(2, 0).ToString();
                    _rc.external_id = Convert.ToInt32(ares.GetValue(3, 0).ToString());
                    _rc.format_file = ares.GetValue(4, 0).ToString();
                    _rc.cache_level = Convert.ToInt32(ares.GetValue(5, 0).ToString());
                    
                    // now get paramters and values, values could be  be componenet only, component in template or component in document
                    // get basecomponent paramters
                    bc_om_component_type ct = new bc_om_component_type();
                    ct.component_id = _rc.type;
                    ct.parameters.component_id = _rc.type;
                    ct.parameters.db_read();
                    _rc.parameters.component_template_parameters = ct.parameters.bc_om_component_parameters;
                    int param_count = _rc.parameters.component_template_parameters.Count;
                    
                    if (mode == bc_component_mode.COMPONENT)
                    {
                        //over lay default paramter values with those past in
                        if (parameters != null)
                        {
                            //bc_core_components_svc.bc_core_component_parameters l = new  bc_core_components_svc.bc_core_component_parameters ();
                            
                            if (parameters.Count != param_count)
                            {
                                _certificate.error_state = true;
                                _certificate.server_errors.Add("Parameter Count Mismatch for type: " + _rc.type.ToString() + " : " + param_count.ToString() + "/" + parameters.Count.ToString());
                            }
                            else
                            {
                                for (int i = 0; i < param_count; i++)
                                {
                                    bc_om_component_parameter bcp;
                                    bcp = (bc_om_component_parameter)_rc.parameters.component_template_parameters[i];
                                    bcp.system_defined = Convert.ToInt32(parameters[i].system_defiend);
                                    bcp.default_value = parameters[i].value;
                                    _rc.parameters.component_template_parameters[i] = bcp;
                                }
                            }
                        }
                    }
                    if (mode == bc_component_mode.TEMPLATE)
                    {
                        //component_id supplied
                        if (_comp_id > 0)
                        {
                            res = db.get_param_values_for_comp_id(_rc.type, _comp_id, ref  _certificate);
                            ares = (Array)res;
                            // check parameter count matches
                            if (ares.GetUpperBound(1)+1 == param_count)
                            {
                                int i;
                                bc_om_component_parameter bcp;

                                for (i = 0; i <= ares.GetUpperBound(1); i++)
                                {
                                    bcp = (bc_om_component_parameter)_rc.parameters.component_template_parameters[i];
                                    bcp.system_defined =Convert.ToInt32(ares.GetValue(0, i).ToString());
                                    bcp.default_value = ares.GetValue(1, i).ToString();
                                    _rc.parameters.component_template_parameters[i] = bcp;
                                }
                            }
                            else
                            {
                                _certificate.error_state = true;
                                _certificate.server_errors.Add("Parameter Count Mismatch for type: " + _rc.type.ToString() + " and sub_component: " + _comp_id.ToString() + " : " + param_count.ToString() + "/" + ares.GetUpperBound(1).ToString() );
                            }
                        }
                        ////template_id supplied can only do if one component exists.
                        else if (_template_id > 0 )
                        {

                        }

                    }
                    ////over lay system defined and default value with component in document
                    else if (mode == bc_component_mode.DOCUMENT)
                    {


                    }

                }
                else
                {
                    _certificate.error_state = true;
                    _certificate.server_errors.Add("Failed to retrive component metadata for: " + _rc.type.ToString());
                }
            }
            catch (Exception e)
            {
                _certificate.error_state = true;
                _certificate.server_errors.Add("Failed to retrieve component metadata for: " + e.Message.ToString());
            }
        }

        class bc_component_medata_db
        {
            bc_cs_db_services gdb = new bc_cs_db_services();
            public object get_metadata(long id, ref bc_cs_security.certificate certificate)
            {
                string sql = "exec dbo.bc_core_get_config_for_component " + id.ToString();
                return gdb.executesql(sql, ref certificate);
            }
            public object get_param_values_for_comp_id(long id, long comp_id, ref bc_cs_security.certificate certificate)
            {
                string sql = "exec dbo.bc_core_get_component_parameters_from_comp_id " + comp_id.ToString() + "," + id.ToString();
                return gdb.executesql(sql, ref certificate);
            }
        }

    }
}