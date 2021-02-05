namespace BlocNotasToDatagridview
{
    partial class preguntas
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(preguntas));
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ronda_pregunta = new System.Windows.Forms.Label();
            this.ParticipanteA = new System.Windows.Forms.Label();
            this.participanteB = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.axAcroPDF1 = new AxAcroPDFLib.AxAcroPDF();
            this.panelOpciones = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnPausa = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnIgual = new System.Windows.Forms.Button();
            this.btnSig = new System.Windows.Forms.Button();
            this.listaPreguntas = new System.Windows.Forms.ComboBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.txtSeg = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMin = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.panel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axAcroPDF1)).BeginInit();
            this.panelOpciones.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tableLayoutPanel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(800, 73);
            this.panel2.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.Controls.Add(this.ronda_pregunta, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.ParticipanteA, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.participanteB, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 73);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // ronda_pregunta
            // 
            this.ronda_pregunta.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ronda_pregunta.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ronda_pregunta.Font = new System.Drawing.Font("Nirmala UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ronda_pregunta.Location = new System.Drawing.Point(298, 0);
            this.ronda_pregunta.Name = "ronda_pregunta";
            this.ronda_pregunta.Size = new System.Drawing.Size(201, 73);
            this.ronda_pregunta.TabIndex = 3;
            this.ronda_pregunta.Text = "Ronda 1 Pregunta\r\n";
            this.ronda_pregunta.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ParticipanteA
            // 
            this.ParticipanteA.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ParticipanteA.AutoSize = true;
            this.ParticipanteA.Font = new System.Drawing.Font("Nirmala UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ParticipanteA.Location = new System.Drawing.Point(3, 20);
            this.ParticipanteA.Name = "ParticipanteA";
            this.ParticipanteA.Size = new System.Drawing.Size(174, 32);
            this.ParticipanteA.TabIndex = 0;
            this.ParticipanteA.Text = "Participante 1";
            this.ParticipanteA.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // participanteB
            // 
            this.participanteB.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.participanteB.AutoSize = true;
            this.participanteB.Font = new System.Drawing.Font("Nirmala UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.participanteB.Location = new System.Drawing.Point(622, 20);
            this.participanteB.Name = "participanteB";
            this.participanteB.Size = new System.Drawing.Size(175, 32);
            this.participanteB.TabIndex = 2;
            this.participanteB.Text = "Participante B";
            this.participanteB.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.axAcroPDF1);
            this.panel1.Controls.Add(this.panelOpciones);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 450);
            this.panel1.TabIndex = 0;
            // 
            // axAcroPDF1
            // 
            this.axAcroPDF1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.axAcroPDF1.Enabled = true;
            this.axAcroPDF1.Location = new System.Drawing.Point(0, 73);
            this.axAcroPDF1.Name = "axAcroPDF1";
            this.axAcroPDF1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axAcroPDF1.OcxState")));
            this.axAcroPDF1.Size = new System.Drawing.Size(800, 329);
            this.axAcroPDF1.TabIndex = 2;
            this.axAcroPDF1.Visible = false;
            // 
            // panelOpciones
            // 
            this.panelOpciones.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.panelOpciones.Controls.Add(this.tableLayoutPanel2);
            this.panelOpciones.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelOpciones.Location = new System.Drawing.Point(0, 402);
            this.panelOpciones.Name = "panelOpciones";
            this.panelOpciones.Size = new System.Drawing.Size(800, 48);
            this.panelOpciones.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.Silver;
            this.tableLayoutPanel2.ColumnCount = 8;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.625F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.875F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.5F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.5F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.125F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.375F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7F));
            this.tableLayoutPanel2.Controls.Add(this.btnPausa, 7, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnPlay, 6, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnIgual, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnSig, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.listaPreguntas, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel3, 3, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(800, 48);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // btnPausa
            // 
            this.btnPausa.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPausa.BackColor = System.Drawing.Color.Silver;
            this.btnPausa.Enabled = false;
            this.btnPausa.FlatAppearance.BorderSize = 0;
            this.btnPausa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPausa.Image = ((System.Drawing.Image)(resources.GetObject("btnPausa.Image")));
            this.btnPausa.Location = new System.Drawing.Point(747, 3);
            this.btnPausa.Name = "btnPausa";
            this.btnPausa.Size = new System.Drawing.Size(50, 42);
            this.btnPausa.TabIndex = 7;
            this.btnPausa.UseVisualStyleBackColor = false;
            this.btnPausa.Click += new System.EventHandler(this.btnPausa_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPlay.BackColor = System.Drawing.Color.Silver;
            this.btnPlay.Enabled = false;
            this.btnPlay.FlatAppearance.BorderSize = 0;
            this.btnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlay.Image = ((System.Drawing.Image)(resources.GetObject("btnPlay.Image")));
            this.btnPlay.Location = new System.Drawing.Point(688, 3);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(53, 42);
            this.btnPlay.TabIndex = 6;
            this.btnPlay.UseVisualStyleBackColor = false;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnIgual
            // 
            this.btnIgual.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnIgual.BackColor = System.Drawing.Color.Silver;
            this.btnIgual.Enabled = false;
            this.btnIgual.FlatAppearance.BorderSize = 0;
            this.btnIgual.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIgual.Image = ((System.Drawing.Image)(resources.GetObject("btnIgual.Image")));
            this.btnIgual.Location = new System.Drawing.Point(51, 3);
            this.btnIgual.Name = "btnIgual";
            this.btnIgual.Size = new System.Drawing.Size(47, 42);
            this.btnIgual.TabIndex = 1;
            this.btnIgual.UseVisualStyleBackColor = false;
            this.btnIgual.Click += new System.EventHandler(this.btnIgual_Click);
            // 
            // btnSig
            // 
            this.btnSig.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSig.BackColor = System.Drawing.Color.Silver;
            this.btnSig.FlatAppearance.BorderSize = 0;
            this.btnSig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSig.Image = ((System.Drawing.Image)(resources.GetObject("btnSig.Image")));
            this.btnSig.Location = new System.Drawing.Point(3, 3);
            this.btnSig.Name = "btnSig";
            this.btnSig.Size = new System.Drawing.Size(42, 42);
            this.btnSig.TabIndex = 0;
            this.btnSig.UseVisualStyleBackColor = false;
            this.btnSig.Click += new System.EventHandler(this.button1_Click);
            // 
            // listaPreguntas
            // 
            this.listaPreguntas.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.listaPreguntas.FormattingEnabled = true;
            this.listaPreguntas.Location = new System.Drawing.Point(104, 13);
            this.listaPreguntas.Name = "listaPreguntas";
            this.listaPreguntas.Size = new System.Drawing.Size(41, 21);
            this.listaPreguntas.TabIndex = 8;
            this.listaPreguntas.SelectedIndexChanged += new System.EventHandler(this.listaPreguntas_SelectedIndexChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.tableLayoutPanel3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(151, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(414, 42);
            this.panel3.TabIndex = 9;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.13527F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.004831F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.61836F));
            this.tableLayoutPanel3.Controls.Add(this.txtSeg, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtMin, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(414, 42);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // txtSeg
            // 
            this.txtSeg.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtSeg.AutoSize = true;
            this.txtSeg.Font = new System.Drawing.Font("Microsoft Tai Le", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSeg.Location = new System.Drawing.Point(223, 0);
            this.txtSeg.Name = "txtSeg";
            this.txtSeg.Size = new System.Drawing.Size(54, 41);
            this.txtSeg.TabIndex = 3;
            this.txtSeg.Text = "00";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Tai Le", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(194, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 41);
            this.label1.TabIndex = 2;
            this.label1.Text = ":";
            // 
            // txtMin
            // 
            this.txtMin.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.txtMin.AutoSize = true;
            this.txtMin.Font = new System.Drawing.Font("Microsoft Tai Le", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMin.Location = new System.Drawing.Point(152, 0);
            this.txtMin.Name = "txtMin";
            this.txtMin.Size = new System.Drawing.Size(36, 41);
            this.txtMin.TabIndex = 1;
            this.txtMin.Text = "3";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // preguntas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "preguntas";
            this.Load += new System.EventHandler(this.preguntas_Load);
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axAcroPDF1)).EndInit();
            this.panelOpciones.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label ronda_pregunta;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelOpciones;
        private System.Windows.Forms.Button btnSig;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnPausa;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.ComboBox listaPreguntas;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Timer timer1;
        public System.Windows.Forms.Label participanteB;
        public System.Windows.Forms.Label ParticipanteA;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        public AxAcroPDFLib.AxAcroPDF axAcroPDF1;
        public System.Windows.Forms.Label txtSeg;
        public System.Windows.Forms.Label txtMin;
        public System.Windows.Forms.Button btnIgual;
    }
}