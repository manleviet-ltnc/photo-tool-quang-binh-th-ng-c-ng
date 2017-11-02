namespace MyAlbumEditor
{
    partial class AlbumCalendar
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
            this.calDates = new System.Windows.Forms.MonthCalendar();
            this.SuspendLayout();
            // 
            // calDates
            // 
            this.calDates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.calDates.Location = new System.Drawing.Point(0, 0);
            this.calDates.MaxSelectionCount = 1;
            this.calDates.Name = "calDates";
            this.calDates.ShowToday = false;
            this.calDates.TabIndex = 0;
            this.calDates.MouseDown += new System.Windows.Forms.MouseEventHandler(this.calDates_MouseDown);
            // 
            // AlbumCalendar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.calDates);
            this.Name = "AlbumCalendar";
            this.Size = new System.Drawing.Size(415, 170);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MonthCalendar calDates;
    }
}
