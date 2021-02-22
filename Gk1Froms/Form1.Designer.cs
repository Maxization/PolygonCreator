namespace Gk1Froms
{
    partial class Form1
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.polygonDelete = new System.Windows.Forms.RadioButton();
            this.edgeMove = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.VertexDelete = new System.Windows.Forms.RadioButton();
            this.VertexMove = new System.Windows.Forms.RadioButton();
            this.VertexAdd = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.PolygonMove = new System.Windows.Forms.RadioButton();
            this.PolygonAdd = new System.Windows.Forms.RadioButton();
            this.addElipse = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.elipseMove = new System.Windows.Forms.RadioButton();
            this.sizeElipse = new System.Windows.Forms.RadioButton();
            this.deleteElipse = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 27.77778F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 72.22222F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 450);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 128);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(794, 319);
            this.panel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(794, 319);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(272, 119);
            this.panel2.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.deleteElipse);
            this.groupBox1.Controls.Add(this.sizeElipse);
            this.groupBox1.Controls.Add(this.elipseMove);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.addElipse);
            this.groupBox1.Controls.Add(this.polygonDelete);
            this.groupBox1.Controls.Add(this.edgeMove);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.VertexDelete);
            this.groupBox1.Controls.Add(this.VertexMove);
            this.groupBox1.Controls.Add(this.VertexAdd);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.PolygonMove);
            this.groupBox1.Controls.Add(this.PolygonAdd);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(272, 119);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Edit";
            // 
            // polygonDelete
            // 
            this.polygonDelete.AutoSize = true;
            this.polygonDelete.Location = new System.Drawing.Point(9, 95);
            this.polygonDelete.Name = "polygonDelete";
            this.polygonDelete.Size = new System.Drawing.Size(56, 17);
            this.polygonDelete.TabIndex = 10;
            this.polygonDelete.Text = "Delete";
            this.polygonDelete.UseVisualStyleBackColor = true;
            this.polygonDelete.CheckedChanged += new System.EventHandler(this.polygonDelete_CheckedChanged);
            // 
            // edgeMove
            // 
            this.edgeMove.AutoSize = true;
            this.edgeMove.Location = new System.Drawing.Point(156, 48);
            this.edgeMove.Name = "edgeMove";
            this.edgeMove.Size = new System.Drawing.Size(52, 17);
            this.edgeMove.TabIndex = 9;
            this.edgeMove.Text = "Move";
            this.edgeMove.UseVisualStyleBackColor = true;
            this.edgeMove.CheckedChanged += new System.EventHandler(this.edgeMove_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(153, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "Edge";
            // 
            // VertexDelete
            // 
            this.VertexDelete.AutoSize = true;
            this.VertexDelete.Location = new System.Drawing.Point(87, 95);
            this.VertexDelete.Name = "VertexDelete";
            this.VertexDelete.Size = new System.Drawing.Size(56, 17);
            this.VertexDelete.TabIndex = 7;
            this.VertexDelete.Text = "Delete";
            this.VertexDelete.UseVisualStyleBackColor = true;
            this.VertexDelete.CheckedChanged += new System.EventHandler(this.VertexDelete_CheckedChanged);
            // 
            // VertexMove
            // 
            this.VertexMove.AutoSize = true;
            this.VertexMove.Location = new System.Drawing.Point(87, 72);
            this.VertexMove.Name = "VertexMove";
            this.VertexMove.Size = new System.Drawing.Size(52, 17);
            this.VertexMove.TabIndex = 6;
            this.VertexMove.Text = "Move";
            this.VertexMove.UseVisualStyleBackColor = true;
            this.VertexMove.CheckedChanged += new System.EventHandler(this.VertexMove_CheckedChanged);
            // 
            // VertexAdd
            // 
            this.VertexAdd.AutoSize = true;
            this.VertexAdd.Location = new System.Drawing.Point(87, 48);
            this.VertexAdd.Name = "VertexAdd";
            this.VertexAdd.Size = new System.Drawing.Size(44, 17);
            this.VertexAdd.TabIndex = 5;
            this.VertexAdd.Text = "Add";
            this.VertexAdd.UseVisualStyleBackColor = true;
            this.VertexAdd.CheckedChanged += new System.EventHandler(this.VertexAdd_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(6, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Polygon";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(84, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Vertex";
            // 
            // PolygonMove
            // 
            this.PolygonMove.AutoSize = true;
            this.PolygonMove.Location = new System.Drawing.Point(9, 72);
            this.PolygonMove.Name = "PolygonMove";
            this.PolygonMove.Size = new System.Drawing.Size(52, 17);
            this.PolygonMove.TabIndex = 1;
            this.PolygonMove.Text = "Move";
            this.PolygonMove.UseVisualStyleBackColor = true;
            this.PolygonMove.CheckedChanged += new System.EventHandler(this.PolygonMove_CheckedChanged);
            // 
            // PolygonAdd
            // 
            this.PolygonAdd.AutoSize = true;
            this.PolygonAdd.Checked = true;
            this.PolygonAdd.Location = new System.Drawing.Point(9, 48);
            this.PolygonAdd.Name = "PolygonAdd";
            this.PolygonAdd.Size = new System.Drawing.Size(44, 17);
            this.PolygonAdd.TabIndex = 0;
            this.PolygonAdd.TabStop = true;
            this.PolygonAdd.Text = "Add";
            this.PolygonAdd.UseVisualStyleBackColor = true;
            this.PolygonAdd.CheckedChanged += new System.EventHandler(this.PolygonAdd_CheckedChanged);
            // 
            // addElipse
            // 
            this.addElipse.AutoSize = true;
            this.addElipse.Location = new System.Drawing.Point(212, 36);
            this.addElipse.Name = "addElipse";
            this.addElipse.Size = new System.Drawing.Size(44, 17);
            this.addElipse.TabIndex = 11;
            this.addElipse.TabStop = true;
            this.addElipse.Text = "Add";
            this.addElipse.UseVisualStyleBackColor = true;
            this.addElipse.CheckedChanged += new System.EventHandler(this.addElipse_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.Location = new System.Drawing.Point(210, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 17);
            this.label4.TabIndex = 12;
            this.label4.Text = "Elipse";
            // 
            // elipseMove
            // 
            this.elipseMove.AutoSize = true;
            this.elipseMove.Location = new System.Drawing.Point(212, 59);
            this.elipseMove.Name = "elipseMove";
            this.elipseMove.Size = new System.Drawing.Size(52, 17);
            this.elipseMove.TabIndex = 13;
            this.elipseMove.Text = "Move";
            this.elipseMove.UseVisualStyleBackColor = true;
            this.elipseMove.CheckedChanged += new System.EventHandler(this.elipseMove_CheckedChanged);
            // 
            // sizeElipse
            // 
            this.sizeElipse.AutoSize = true;
            this.sizeElipse.Location = new System.Drawing.Point(212, 76);
            this.sizeElipse.Name = "sizeElipse";
            this.sizeElipse.Size = new System.Drawing.Size(45, 17);
            this.sizeElipse.TabIndex = 14;
            this.sizeElipse.Text = "Size";
            this.sizeElipse.UseVisualStyleBackColor = true;
            this.sizeElipse.CheckedChanged += new System.EventHandler(this.sizeElipse_CheckedChanged);
            // 
            // deleteElipse
            // 
            this.deleteElipse.AutoSize = true;
            this.deleteElipse.Location = new System.Drawing.Point(212, 95);
            this.deleteElipse.Name = "deleteElipse";
            this.deleteElipse.Size = new System.Drawing.Size(56, 17);
            this.deleteElipse.TabIndex = 15;
            this.deleteElipse.Text = "Delete";
            this.deleteElipse.UseVisualStyleBackColor = true;
            this.deleteElipse.CheckedChanged += new System.EventHandler(this.deleteElipse_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "PolygonCreator";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton polygonDelete;
        private System.Windows.Forms.RadioButton edgeMove;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton VertexDelete;
        private System.Windows.Forms.RadioButton VertexMove;
        private System.Windows.Forms.RadioButton VertexAdd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton PolygonMove;
        private System.Windows.Forms.RadioButton PolygonAdd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton addElipse;
        private System.Windows.Forms.RadioButton elipseMove;
        private System.Windows.Forms.RadioButton sizeElipse;
        private System.Windows.Forms.RadioButton deleteElipse;
    }
}

