namespace Calculator
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.richTextBox_left = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.richTextBox_right = new System.Windows.Forms.RichTextBox();
            this.richTextBox_log = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox_left.Location = new System.Drawing.Point(12, 12);
            this.richTextBox_left.Name = "richTextBox1";
            this.richTextBox_left.Size = new System.Drawing.Size(245, 209);
            this.richTextBox_left.TabIndex = 0;
            this.richTextBox_left.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(518, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Clear";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button_clearTextBoxes);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(518, 41);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "DumpLogs";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button_dumpLogs);
            // 
            // richTextBox2
            // 
            this.richTextBox_right.Location = new System.Drawing.Point(267, 12);
            this.richTextBox_right.Name = "richTextBox2";
            this.richTextBox_right.Size = new System.Drawing.Size(245, 209);
            this.richTextBox_right.TabIndex = 4;
            this.richTextBox_right.Text = "";
            // 
            // richTextBox3
            // 
            this.richTextBox_log.Location = new System.Drawing.Point(12, 227);
            this.richTextBox_log.Name = "richTextBox3";
            this.richTextBox_log.Size = new System.Drawing.Size(500, 172);
            this.richTextBox_log.TabIndex = 5;
            this.richTextBox_log.Text = "";
            this.richTextBox_log.TextChanged += new System.EventHandler(this.richTextBox_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 408);
            this.Controls.Add(this.richTextBox_log);
            this.Controls.Add(this.richTextBox_right);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBox_left);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox_left;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RichTextBox richTextBox_right;
        private System.Windows.Forms.RichTextBox richTextBox_log;
    }
}

