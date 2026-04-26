namespace Horarios
{
    partial class MenuPrincipal
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
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            button3 = new Button();
            button2 = new Button();
            button1 = new Button();
            comboBoxMateria = new ComboBox();
            comboBoxMaestro = new ComboBox();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            comboBox1 = new ComboBox();
            dataGridView1 = new DataGridView();
            tabPage3 = new TabPage();
            label9 = new Label();
            label8 = new Label();
            button10 = new Button();
            button9 = new Button();
            button8 = new Button();
            button7 = new Button();
            textBox6 = new TextBox();
            textBox5 = new TextBox();
            tabPage2 = new TabPage();
            button6 = new Button();
            button5 = new Button();
            button4 = new Button();
            textBox4 = new TextBox();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            textBox3 = new TextBox();
            textBox2 = new TextBox();
            textBox1 = new TextBox();
            dataGridViewMaestros = new DataGridView();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            tabPage3.SuspendLayout();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewMaestros).BeginInit();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(12, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(776, 426);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(button3);
            tabPage1.Controls.Add(button2);
            tabPage1.Controls.Add(button1);
            tabPage1.Controls.Add(comboBoxMateria);
            tabPage1.Controls.Add(comboBoxMaestro);
            tabPage1.Controls.Add(label7);
            tabPage1.Controls.Add(label6);
            tabPage1.Controls.Add(label5);
            tabPage1.Controls.Add(comboBox1);
            tabPage1.Controls.Add(dataGridView1);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(768, 393);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Horarios";
            tabPage1.UseVisualStyleBackColor = true;
            tabPage1.Click += tabPage1_Click;
            // 
            // button3
            // 
            button3.Location = new Point(635, 54);
            button3.Name = "button3";
            button3.Size = new Size(94, 29);
            button3.TabIndex = 9;
            button3.Text = "Modificar";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button2
            // 
            button2.Location = new Point(635, 89);
            button2.Name = "button2";
            button2.Size = new Size(94, 29);
            button2.TabIndex = 8;
            button2.Text = "Eliminar";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Location = new Point(635, 124);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 7;
            button1.Text = "Asignar";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // comboBoxMateria
            // 
            comboBoxMateria.FormattingEnabled = true;
            comboBoxMateria.Location = new Point(386, 38);
            comboBoxMateria.Name = "comboBoxMateria";
            comboBoxMateria.Size = new Size(151, 28);
            comboBoxMateria.TabIndex = 6;
            // 
            // comboBoxMaestro
            // 
            comboBoxMaestro.FormattingEnabled = true;
            comboBoxMaestro.Location = new Point(184, 37);
            comboBoxMaestro.Name = "comboBoxMaestro";
            comboBoxMaestro.Size = new Size(151, 28);
            comboBoxMaestro.TabIndex = 5;
            comboBoxMaestro.SelectionChangeCommitted += comboBoxMaestro_SelectionChangeCommitted;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(438, 15);
            label7.Name = "label7";
            label7.Size = new Size(60, 20);
            label7.TabIndex = 4;
            label7.Text = "Materia";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(238, 15);
            label6.Name = "label6";
            label6.Size = new Size(63, 20);
            label6.TabIndex = 3;
            label6.Text = "Maestro";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 15);
            label5.Name = "label5";
            label5.Size = new Size(50, 20);
            label5.TabIndex = 2;
            label5.Text = "Grupo";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(6, 38);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(151, 28);
            comboBox1.TabIndex = 1;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(6, 180);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(756, 207);
            dataGridView1.TabIndex = 0;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(label9);
            tabPage3.Controls.Add(label8);
            tabPage3.Controls.Add(button10);
            tabPage3.Controls.Add(button9);
            tabPage3.Controls.Add(button8);
            tabPage3.Controls.Add(button7);
            tabPage3.Controls.Add(textBox6);
            tabPage3.Controls.Add(textBox5);
            tabPage3.Location = new Point(4, 29);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(768, 393);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Clases";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(27, 93);
            label9.Name = "label9";
            label9.Size = new Size(74, 20);
            label9.TabIndex = 8;
            label9.Text = "Matricula:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(27, 43);
            label8.Name = "label8";
            label8.Size = new Size(67, 20);
            label8.TabIndex = 7;
            label8.Text = "Nombre:";
            // 
            // button10
            // 
            button10.Location = new Point(656, 329);
            button10.Name = "button10";
            button10.Size = new Size(94, 29);
            button10.TabIndex = 6;
            button10.Text = "Consultas";
            button10.UseVisualStyleBackColor = true;
            // 
            // button9
            // 
            button9.Location = new Point(552, 329);
            button9.Name = "button9";
            button9.Size = new Size(98, 30);
            button9.TabIndex = 5;
            button9.Text = "Eliminar";
            button9.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            button8.Location = new Point(443, 329);
            button8.Name = "button8";
            button8.Size = new Size(94, 29);
            button8.TabIndex = 4;
            button8.Text = "Modificar";
            button8.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            button7.Location = new Point(343, 329);
            button7.Name = "button7";
            button7.Size = new Size(94, 29);
            button7.TabIndex = 3;
            button7.Text = "Registrar";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // textBox6
            // 
            textBox6.Location = new Point(127, 86);
            textBox6.Name = "textBox6";
            textBox6.Size = new Size(125, 27);
            textBox6.TabIndex = 1;
            // 
            // textBox5
            // 
            textBox5.Location = new Point(127, 36);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(125, 27);
            textBox5.TabIndex = 0;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(button6);
            tabPage2.Controls.Add(button5);
            tabPage2.Controls.Add(button4);
            tabPage2.Controls.Add(textBox4);
            tabPage2.Controls.Add(label4);
            tabPage2.Controls.Add(label3);
            tabPage2.Controls.Add(label2);
            tabPage2.Controls.Add(label1);
            tabPage2.Controls.Add(textBox3);
            tabPage2.Controls.Add(textBox2);
            tabPage2.Controls.Add(textBox1);
            tabPage2.Controls.Add(dataGridViewMaestros);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(768, 393);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Maestros";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            button6.Location = new Point(649, 72);
            button6.Name = "button6";
            button6.Size = new Size(94, 29);
            button6.TabIndex = 11;
            button6.Text = "Eliminar";
            button6.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            button5.Location = new Point(649, 111);
            button5.Name = "button5";
            button5.Size = new Size(94, 29);
            button5.TabIndex = 10;
            button5.Text = "Modificar";
            button5.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Location = new Point(649, 147);
            button4.Name = "button4";
            button4.Size = new Size(94, 29);
            button4.TabIndex = 9;
            button4.Text = "Registrar";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(79, 113);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(125, 27);
            textBox4.TabIndex = 8;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(7, 116);
            label4.Name = "label4";
            label4.Size = new Size(66, 20);
            label4.TabIndex = 7;
            label4.Text = "Numero:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(18, 83);
            label3.Name = "label3";
            label3.Size = new Size(58, 20);
            label3.TabIndex = 6;
            label3.Text = "Cedula:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(-1, 48);
            label2.Name = "label2";
            label2.Size = new Size(74, 20);
            label2.TabIndex = 5;
            label2.Text = "Matricula:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 12);
            label1.Name = "label1";
            label1.Size = new Size(67, 20);
            label1.TabIndex = 4;
            label1.Text = "Nombre:";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(79, 9);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(125, 27);
            textBox3.TabIndex = 3;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(79, 80);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(125, 27);
            textBox2.TabIndex = 2;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(79, 45);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(125, 27);
            textBox1.TabIndex = 1;
            // 
            // dataGridViewMaestros
            // 
            dataGridViewMaestros.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewMaestros.Location = new Point(9, 199);
            dataGridViewMaestros.Name = "dataGridViewMaestros";
            dataGridViewMaestros.RowHeadersWidth = 51;
            dataGridViewMaestros.Size = new Size(756, 188);
            dataGridViewMaestros.TabIndex = 0;
            // 
            // MenuPrincipal
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tabControl1);
            Name = "MenuPrincipal";
            Text = "MenuPrincipal";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewMaestros).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage3;
        private TabPage tabPage2;
        private TextBox textBox3;
        private TextBox textBox2;
        private TextBox textBox1;
        private DataGridView dataGridViewMaestros;
        private TextBox textBox4;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private DataGridView dataGridView1;
        private Button button1;
        private ComboBox comboBoxMateria;
        private ComboBox comboBoxMaestro;
        private Label label7;
        private Label label6;
        private Label label5;
        private ComboBox comboBox1;
        private Button button3;
        private Button button2;
        private Button button4;
        private Button button6;
        private Button button5;
        private TextBox textBox6;
        private TextBox textBox5;
        private Button button10;
        private Button button9;
        private Button button8;
        private Button button7;
        private Label label9;
        private Label label8;
    }
}