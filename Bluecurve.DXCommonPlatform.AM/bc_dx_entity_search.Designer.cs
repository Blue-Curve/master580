namespace Bluecurve.DXCommonPlatform.AM
{
    partial class bc_dx_entity_search
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(bc_dx_entity_search));
            this.uxfilter = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lentities = new DevExpress.XtraEditors.ListBoxControl();
            this.rallmine = new DevExpress.XtraEditors.RadioGroup();
            this.tsearch = new DevExpress.XtraEditors.TextEdit();
            this.pictureEdit2 = new DevExpress.XtraEditors.PictureEdit();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.uxclass = new DevExpress.XtraEditors.ComboBoxEdit();
            this.timer1 = new System.Windows.Forms.Timer();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection();
            this.pfilter = new DevExpress.XtraEditors.PanelControl();
            this.uxfilteroptions = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.uxfilter.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lentities)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rallmine.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tsearch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uxclass.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pfilter)).BeginInit();
            this.pfilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uxfilteroptions.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // uxfilter
            // 
            this.uxfilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxfilter.Location = new System.Drawing.Point(9, 20);
            this.uxfilter.Name = "uxfilter";
            this.uxfilter.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.uxfilter.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.uxfilter.Size = new System.Drawing.Size(327, 20);
            this.uxfilter.TabIndex = 16;
            this.uxfilter.SelectedIndexChanged += new System.EventHandler(this.uxfilter_SelectedIndexChanged_1);
            // 
            // lentities
            // 
            this.lentities.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lentities.Location = new System.Drawing.Point(12, 121);
            this.lentities.Name = "lentities";
            this.lentities.Size = new System.Drawing.Size(327, 183);
            this.lentities.TabIndex = 15;
            this.lentities.SelectedIndexChanged += new System.EventHandler(this.lentities_SelectedIndexChanged_1);
            // 
            // rallmine
            // 
            this.rallmine.CausesValidation = false;
            this.rallmine.Location = new System.Drawing.Point(12, 101);
            this.rallmine.Name = "rallmine";
            this.rallmine.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.rallmine.Properties.Appearance.Options.UseBackColor = true;
            this.rallmine.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.rallmine.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Active"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Inactive")});
            this.rallmine.Size = new System.Drawing.Size(131, 21);
            this.rallmine.TabIndex = 14;
            this.rallmine.SelectedIndexChanged += new System.EventHandler(this.rallmine_SelectedIndexChanged_1);
            // 
            // tsearch
            // 
            this.tsearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tsearch.Location = new System.Drawing.Point(55, 325);
            this.tsearch.Name = "tsearch";
            this.tsearch.Size = new System.Drawing.Size(241, 20);
            this.tsearch.TabIndex = 13;
            // 
            // pictureEdit2
            // 
            this.pictureEdit2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureEdit2.EditValue = ((object)(resources.GetObject("pictureEdit2.EditValue")));
            this.pictureEdit2.Location = new System.Drawing.Point(302, 310);
            this.pictureEdit2.Name = "pictureEdit2";
            this.pictureEdit2.Size = new System.Drawing.Size(37, 35);
            this.pictureEdit2.TabIndex = 12;
            this.pictureEdit2.EditValueChanged += new System.EventHandler(this.pictureEdit2_EditValueChanged);
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureEdit1.EditValue = ((object)(resources.GetObject("pictureEdit1.EditValue")));
            this.pictureEdit1.Location = new System.Drawing.Point(12, 310);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Size = new System.Drawing.Size(37, 35);
            this.pictureEdit1.TabIndex = 11;
            // 
            // uxclass
            // 
            this.uxclass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxclass.Location = new System.Drawing.Point(12, 3);
            this.uxclass.Name = "uxclass";
            this.uxclass.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.uxclass.Properties.Sorted = true;
            this.uxclass.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.uxclass.Size = new System.Drawing.Size(327, 20);
            this.uxclass.TabIndex = 10;
            this.uxclass.SelectedIndexChanged += new System.EventHandler(this.uxclass_SelectedIndexChanged);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.Images.SetKeyName(0, "cross.png");
            this.imageCollection1.Images.SetKeyName(1, "tick.png");
            // 
            // pfilter
            // 
            this.pfilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pfilter.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pfilter.Controls.Add(this.labelControl1);
            this.pfilter.Controls.Add(this.uxfilteroptions);
            this.pfilter.Controls.Add(this.uxfilter);
            this.pfilter.Location = new System.Drawing.Point(3, 29);
            this.pfilter.Name = "pfilter";
            this.pfilter.Size = new System.Drawing.Size(341, 69);
            this.pfilter.TabIndex = 17;
            // 
            // uxfilteroptions
            // 
            this.uxfilteroptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxfilteroptions.Enabled = false;
            this.uxfilteroptions.Location = new System.Drawing.Point(9, 46);
            this.uxfilteroptions.Name = "uxfilteroptions";
            this.uxfilteroptions.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.uxfilteroptions.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.uxfilteroptions.Size = new System.Drawing.Size(327, 20);
            this.uxfilteroptions.TabIndex = 17;
            this.uxfilteroptions.SelectedIndexChanged += new System.EventHandler(this.uxfilteroptions_SelectedIndexChanged_1);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(9, 3);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(41, 13);
            this.labelControl1.TabIndex = 18;
            this.labelControl1.Text = "Filter On";
            // 
            // bc_dx_entity_search
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lentities);
            this.Controls.Add(this.rallmine);
            this.Controls.Add(this.tsearch);
            this.Controls.Add(this.pictureEdit2);
            this.Controls.Add(this.pictureEdit1);
            this.Controls.Add(this.uxclass);
            this.Controls.Add(this.pfilter);
            this.Name = "bc_dx_entity_search";
            this.Size = new System.Drawing.Size(347, 358);
            ((System.ComponentModel.ISupportInitialize)(this.uxfilter.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lentities)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rallmine.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tsearch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uxclass.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pfilter)).EndInit();
            this.pfilter.ResumeLayout(false);
            this.pfilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uxfilteroptions.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.ComboBoxEdit uxfilter;
        private DevExpress.XtraEditors.ListBoxControl lentities;
        private DevExpress.XtraEditors.RadioGroup rallmine;
        private DevExpress.XtraEditors.TextEdit tsearch;
        private DevExpress.XtraEditors.PictureEdit pictureEdit2;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraEditors.ComboBoxEdit uxclass;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.Utils.ImageCollection imageCollection1;
        private DevExpress.XtraEditors.PanelControl pfilter;
        private DevExpress.XtraEditors.ComboBoxEdit uxfilteroptions;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}
