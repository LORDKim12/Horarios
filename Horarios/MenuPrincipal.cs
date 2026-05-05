using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Horarios
{
    public partial class MenuPrincipal : Form
    {
        public MenuPrincipal()
        {
            InitializeComponent();
            CargarMaestros();
            ConfigurarGridHorario();
            CargarDatosIniciales();
            CargarDatosAsignacion();
        }



        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }
        private void CargarMaestros()
        {
            Conexion conexion = new Conexion();
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT IdMaestro AS ID, Matricula, Nombre, Cedula, Numero FROM Maestros";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridViewMaestros.DataSource = dt; 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar maestros: " + ex.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("El Nombre y la Matrícula son obligatorios para registrar a un maestro.");
                return;
            }

            
            Conexion conexion = new Conexion();
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Maestros (Nombre, Matricula, Cedula, Numero) VALUES (@nom, @mat, @ced, @num)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    
                    cmd.Parameters.AddWithValue("@nom", textBox3.Text);
                    cmd.Parameters.AddWithValue("@mat", textBox1.Text); 
                    cmd.Parameters.AddWithValue("@ced", textBox2.Text); 
                    cmd.Parameters.AddWithValue("@num", textBox4.Text); 

                    cmd.ExecuteNonQuery(); 

                    MessageBox.Show("Maestro registrado con éxito.");

                    
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    textBox4.Clear();

         
                    CargarMaestros();            
                    CargarMaestrosParaCombo();    
                    CargarDatosAsignacion();      

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar: " + ex.Message);
                }
            }
        }

        private void comboBoxMaestro_SelectionChangeCommitted(object sender, EventArgs e)
        {
            
            if (comboBoxMaestro.SelectedValue != null)
            {
                
                int idMaestroSeleccionado = Convert.ToInt32(comboBoxMaestro.SelectedValue);

                
                CargarMateriasPorMaestro(idMaestroSeleccionado);
            }
        }
        private void CargarMateriasPorMaestro(int idMaestro)
        {
            Conexion conexion = new Conexion();
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    
                    string query = @"
                SELECT m.IdMateria, m.NombreMateria 
                FROM Materias m
                INNER JOIN Maestros_Materias mm ON m.IdMateria = mm.IdMateria
                WHERE mm.IdMaestro = @idMaestro";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idMaestro", idMaestro);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dtMaterias = new DataTable();
                    adapter.Fill(dtMaterias);

                    
                    comboBoxMateria.DataSource = dtMaterias;
                    comboBoxMateria.DisplayMember = "NombreMateria"; 
                    comboBoxMateria.ValueMember = "IdMateria";       

                    
                    comboBoxMateria.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al filtrar materias: " + ex.Message);
                }
            }
        }
        private void ConfigurarGridHorario()
        {
            
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            
            dataGridView1.AllowUserToAddRows = false;    
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;               
            dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect; 
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; 
            dataGridView1.RowHeadersVisible = false;    

            
            dataGridView1.Columns.Add("Hora", "Hora");
            dataGridView1.Columns.Add("Lunes", "Lunes");
            dataGridView1.Columns.Add("Martes", "Martes");
            dataGridView1.Columns.Add("Miercoles", "Miércoles");
            dataGridView1.Columns.Add("Jueves", "Jueves");
            dataGridView1.Columns.Add("Viernes", "Viernes");

           
            dataGridView1.Columns["Hora"].DefaultCellStyle.BackColor = Color.LightGray;
            dataGridView1.Columns["Hora"].DefaultCellStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);
            dataGridView1.Columns["Hora"].Frozen = true; 

            
            string[] bloquesDeHora = {
        "07:00 - 08:00",
        "08:00 - 09:00",
        "09:00 - 10:00",
        "10:00 - 11:00",
        "11:00 - 12:00",
        "12:00 - 13:00",
        "13:00 - 14:00"
    };

            
            foreach (string hora in bloquesDeHora)
            {
                
                dataGridView1.Rows.Add(hora, "", "", "", "", "");
            }
        }

        private void CargarDatosIniciales()
        {
            CargarGrupos();
            CargarMaestrosParaCombo();
        }

        private void CargarGrupos()
        {
            Conexion conexion = new Conexion();
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT IdGrupo, NombreGrupo FROM Grupos";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    
                    comboBox1.DataSource = dt;
                    comboBox1.DisplayMember = "NombreGrupo";
                    comboBox1.ValueMember = "IdGrupo";
                    comboBox1.SelectedIndex = -1; 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar grupos: " + ex.Message);
                }
            }
        }

        private void CargarMaestrosParaCombo()
        {
            Conexion conexion = new Conexion();
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT IdMaestro, Nombre FROM Maestros";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    
                    comboBoxMaestro.DataSource = null;
                    comboBoxMaestro.DataSource = dt;
                    comboBoxMaestro.DisplayMember = "Nombre";
                    comboBoxMaestro.ValueMember = "IdMaestro";
                    comboBoxMaestro.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (comboBox1.SelectedValue == null || comboBoxMaestro.SelectedValue == null || comboBoxMateria.SelectedValue == null)
            {
                MessageBox.Show("Por favor selecciona un Grupo, una Materia y un Maestro.");
                return;
            }

            if (dataGridView1.CurrentCell == null || dataGridView1.CurrentCell.ColumnIndex == 0)
            {
                MessageBox.Show("Por favor selecciona una celda válida de un día en el calendario (no la columna de horas).");
                return;
            }

            
            int idGrupo = Convert.ToInt32(comboBox1.SelectedValue);
            int idMateria = Convert.ToInt32(comboBoxMateria.SelectedValue);
            int idMaestro = Convert.ToInt32(comboBoxMaestro.SelectedValue);

            
            int idDia = dataGridView1.CurrentCell.ColumnIndex;
            
            int idHora = dataGridView1.CurrentCell.RowIndex + 1;

            
            Conexion conexion = new Conexion();
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Horarios (IdGrupo, IdMateria, IdMaestro, IdDia, IdHora) VALUES (@grupo, @mat, @mae, @dia, @hora)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@grupo", idGrupo);
                    cmd.Parameters.AddWithValue("@mat", idMateria);
                    cmd.Parameters.AddWithValue("@mae", idMaestro);
                    cmd.Parameters.AddWithValue("@dia", idDia);
                    cmd.Parameters.AddWithValue("@hora", idHora);

                    cmd.ExecuteNonQuery();

                    
                    dataGridView1.CurrentCell.Value = $"{comboBoxMateria.Text}\n({comboBoxMaestro.Text})";
                    MessageBox.Show("Horario asignado exitosamente.");
                }
                catch (MySqlException ex)
                {
                    
                    if (ex.Number == 1062)
                        MessageBox.Show("Choque de horario: El maestro o el grupo ya tienen una clase asignada en este día y hora.");
                    else
                        MessageBox.Show("Error de base de datos: " + ex.Message);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null || comboBoxMaestro.SelectedValue == null || comboBoxMateria.SelectedValue == null)
            {
                MessageBox.Show("Selecciona la nueva Materia y Maestro para modificar este bloque.");
                return;
            }

            if (dataGridView1.CurrentCell.ColumnIndex == 0) return;

            int idGrupo = Convert.ToInt32(comboBox1.SelectedValue);
            int idMateria = Convert.ToInt32(comboBoxMateria.SelectedValue);
            int idMaestro = Convert.ToInt32(comboBoxMaestro.SelectedValue);
            int idDia = dataGridView1.CurrentCell.ColumnIndex;
            int idHora = dataGridView1.CurrentCell.RowIndex + 1;

            Conexion conexion = new Conexion();
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE Horarios SET IdMateria = @mat, IdMaestro = @mae WHERE IdGrupo = @grupo AND IdDia = @dia AND IdHora = @hora";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@mat", idMateria);
                    cmd.Parameters.AddWithValue("@mae", idMaestro);
                    cmd.Parameters.AddWithValue("@grupo", idGrupo);
                    cmd.Parameters.AddWithValue("@dia", idDia);
                    cmd.Parameters.AddWithValue("@hora", idHora);

                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        dataGridView1.CurrentCell.Value = $"{comboBoxMateria.Text}\n({comboBoxMaestro.Text})";
                        MessageBox.Show("Horario modificado.");
                    }
                    else
                    {
                        MessageBox.Show("No hay ninguna clase asignada en este bloque para modificar. Usa 'Asignar' en su lugar.");
                    }
                }
                catch (MySqlException ex)
                {
                    if (ex.Number == 1062) MessageBox.Show("Choque de horario: El nuevo maestro ya está ocupado a esta hora.");
                    else MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null || dataGridView1.CurrentCell.ColumnIndex == 0)
            {
                MessageBox.Show("Asegúrate de seleccionar un Grupo y una celda del horario.");
                return;
            }

            int idGrupo = Convert.ToInt32(comboBox1.SelectedValue);
            int idDia = dataGridView1.CurrentCell.ColumnIndex;
            int idHora = dataGridView1.CurrentCell.RowIndex + 1;

            DialogResult dialogResult = MessageBox.Show("¿Estás seguro de que deseas vaciar este bloque de horario?", "Confirmar Eliminación", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Conexion conexion = new Conexion();
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM Horarios WHERE IdGrupo = @grupo AND IdDia = @dia AND IdHora = @hora";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@grupo", idGrupo);
                        cmd.Parameters.AddWithValue("@dia", idDia);
                        cmd.Parameters.AddWithValue("@hora", idHora);

                        int filasAfectadas = cmd.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            dataGridView1.CurrentCell.Value = ""; 
                            MessageBox.Show("Clase eliminada del horario.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar: " + ex.Message);
                    }
                }
            }
        }

        private void dataGridViewMaestros_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridViewMaestros.Rows[e.RowIndex];

                
                idMaestroSeleccionado = Convert.ToInt32(fila.Cells["ID"].Value);

                
                textBox3.Text = fila.Cells["Nombre"].Value.ToString();
                textBox1.Text = fila.Cells["Matricula"].Value.ToString();
                textBox2.Text = fila.Cells["Cedula"].Value.ToString();
                textBox4.Text = fila.Cells["Numero"].Value.ToString();
            }
        }
        private int idMaestroSeleccionado = 0;

        
        private void LimpiarCamposMaestros()
        {
            textBox3.Clear(); 
            textBox1.Clear(); 
            textBox2.Clear(); 
            textBox4.Clear(); 
            idMaestroSeleccionado = 0; 
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            if (idMaestroSeleccionado == 0)
            {
                MessageBox.Show("Por favor, selecciona un maestro de la tabla primero.");
                return;
            }

            Conexion conexion = new Conexion();
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE Maestros SET Nombre = @nom, Matricula = @mat, Cedula = @ced, Numero = @num WHERE IdMaestro = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@nom", textBox3.Text);
                    cmd.Parameters.AddWithValue("@mat", textBox1.Text);
                    cmd.Parameters.AddWithValue("@ced", textBox2.Text);
                    cmd.Parameters.AddWithValue("@num", textBox4.Text);
                    cmd.Parameters.AddWithValue("@id", idMaestroSeleccionado);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Datos del maestro actualizados correctamente.");

                    LimpiarCamposMaestros();
                    CargarMaestros(); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al modificar: " + ex.Message);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (idMaestroSeleccionado == 0)
            {
                MessageBox.Show("Por favor, selecciona un maestro de la tabla primero.");
                return;
            }

            
            DialogResult confirmacion = MessageBox.Show("¿Estás seguro de que deseas eliminar a este maestro?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmacion == DialogResult.Yes)
            {
                Conexion conexion = new Conexion();
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM Maestros WHERE IdMaestro = @id";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", idMaestroSeleccionado);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Maestro eliminado con éxito.");

                        LimpiarCamposMaestros();
                        CargarMaestros();
                    }
                    catch (MySqlException ex)
                    {
                        
                        if (ex.Number == 1451)
                        {
                            MessageBox.Show("No puedes eliminar a este maestro porque tiene materias u horarios asignados. Primero elimina sus asignaciones.", "Operación Denegada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show("Error en la base de datos: " + ex.Message);
                        }
                    }
                }
            }
        }
        private void CargarDatosAsignacion()
        {
            Conexion conexion = new Conexion();
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();

                    
                    string queryMaestros = "SELECT IdMaestro, Nombre FROM Maestros";
                    MySqlDataAdapter adapterMaestros = new MySqlDataAdapter(queryMaestros, conn);
                    DataTable dtMaestros = new DataTable();
                    adapterMaestros.Fill(dtMaestros);

                    comboBoxMaestrosAsignacion.DataSource = dtMaestros;
                    comboBoxMaestrosAsignacion.DisplayMember = "Nombre";
                    comboBoxMaestrosAsignacion.ValueMember = "IdMaestro";
                    comboBoxMaestrosAsignacion.SelectedIndex = -1; 

                    
                    string queryMaterias = "SELECT IdMateria, NombreMateria FROM Materias";
                    MySqlDataAdapter adapterMaterias = new MySqlDataAdapter(queryMaterias, conn);
                    DataTable dtMaterias = new DataTable();
                    adapterMaterias.Fill(dtMaterias);

                    
                    ((ListBox)checkedListBoxMaterias).DataSource = dtMaterias;
                    ((ListBox)checkedListBoxMaterias).DisplayMember = "NombreMateria";
                    ((ListBox)checkedListBoxMaterias).ValueMember = "IdMateria";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar datos de asignación: " + ex.Message);
                }
            }
        }

        private void comboBoxMaestrosAsignacion_SelectionChangeCommitted(object sender, EventArgs e)
        {
            
            for (int i = 0; i < checkedListBoxMaterias.Items.Count; i++)
            {
                checkedListBoxMaterias.SetItemChecked(i, false);
            }

            if (comboBoxMaestrosAsignacion.SelectedValue == null) return;

            int idMaestroSeleccionado = Convert.ToInt32(comboBoxMaestrosAsignacion.SelectedValue);

            Conexion conexion = new Conexion();
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    
                    string query = "SELECT IdMateria FROM Maestros_Materias WHERE IdMaestro = @idMaestro";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idMaestro", idMaestroSeleccionado);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        
                        while (reader.Read())
                        {
                            int idMateriaQueImparte = Convert.ToInt32(reader["IdMateria"]);

                            
                            for (int i = 0; i < checkedListBoxMaterias.Items.Count; i++)
                            {
                                DataRowView filaItem = (DataRowView)checkedListBoxMaterias.Items[i];
                                int idMateriaLista = Convert.ToInt32(filaItem["IdMateria"]);

                                if (idMateriaLista == idMateriaQueImparte)
                                {
                                    checkedListBoxMaterias.SetItemChecked(i, true);
                                    break; 
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar las materias del maestro: " + ex.Message);
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (comboBoxMaestrosAsignacion.SelectedValue == null)
            {
                MessageBox.Show("Por favor, selecciona un maestro primero.");
                return;
            }

            int idMaestro = Convert.ToInt32(comboBoxMaestrosAsignacion.SelectedValue);

            Conexion conexion = new Conexion();
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();

                    
                    string queryDelete = "DELETE FROM Maestros_Materias WHERE IdMaestro = @idMaestro";
                    MySqlCommand cmdDelete = new MySqlCommand(queryDelete, conn);
                    cmdDelete.Parameters.AddWithValue("@idMaestro", idMaestro);
                    cmdDelete.ExecuteNonQuery();

                   
                    string queryInsert = "INSERT INTO Maestros_Materias (IdMaestro, IdMateria) VALUES (@idMaestro, @idMateria)";
                    MySqlCommand cmdInsert = new MySqlCommand(queryInsert, conn);

                    
                    foreach (object itemChecked in checkedListBoxMaterias.CheckedItems)
                    {
                        
                        DataRowView filaItem = (DataRowView)itemChecked;
                        int idMateria = Convert.ToInt32(filaItem["IdMateria"]);

                        cmdInsert.Parameters.Clear(); 
                        cmdInsert.Parameters.AddWithValue("@idMaestro", idMaestro);
                        cmdInsert.Parameters.AddWithValue("@idMateria", idMateria);

                        cmdInsert.ExecuteNonQuery();
                    }

                    MessageBox.Show("Materias asignadas correctamente al maestro.");

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar asignaciones: " + ex.Message);
                }
            }
        }

        private void dataGridViewMaterias_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridViewMaterias.Rows[e.RowIndex];

                
                idMateriaSeleccionada = Convert.ToInt32(fila.Cells["ID"].Value);

               
                textBox5.Text = fila.Cells["Nombre"].Value.ToString();

              
            }
        }

        private int idMateriaSeleccionada = 0;

        private void LimpiarCamposMaterias()
        {
            
            textBox5.Clear();
            textBox6.Clear();
            idMateriaSeleccionada = 0;
        }

        private void CargarMaterias()
        {
            Conexion conexion = new Conexion();
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    
                    string query = "SELECT IdMateria AS ID, NombreMateria AS Nombre FROM Materias";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridViewMaterias.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar las materias: " + ex.Message);
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (idMateriaSeleccionada == 0)
            {
                MessageBox.Show("Por favor, selecciona una materia de la tabla haciendo clic en ella.");
                return;
            }

            Conexion conexion = new Conexion();
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    
                    string query = "UPDATE Materias SET NombreMateria = @nom WHERE IdMateria = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@nom", textBox5.Text);
                    cmd.Parameters.AddWithValue("@id", idMateriaSeleccionada);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Materia modificada exitosamente.");

                    LimpiarCamposMaterias();
                    CargarMaterias(); 
                    CargarDatosAsignacion(); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al modificar: " + ex.Message);
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (idMateriaSeleccionada == 0)
            {
                MessageBox.Show("Por favor, selecciona una materia de la tabla haciendo clic en ella.");
                return;
            }

            DialogResult confirmacion = MessageBox.Show("¿Estás seguro de que deseas eliminar esta materia?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmacion == DialogResult.Yes)
            {
                Conexion conexion = new Conexion();
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM Materias WHERE IdMateria = @id";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", idMateriaSeleccionada);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Materia eliminada.");

                        LimpiarCamposMaterias();
                        CargarMaterias(); 
                    }
                    catch (MySqlException ex)
                    {
                        
                        if (ex.Number == 1451)
                        {
                            MessageBox.Show("No puedes eliminar esta materia porque ya está asignada a un maestro o a un horario. Quita las asignaciones primero.");
                        }
                        else
                        {
                            MessageBox.Show("Error en la base de datos: " + ex.Message);
                        }
                    }
                }
            }
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox5.Text))
            {
                MessageBox.Show("El nombre de la materia es obligatorio.");
                return;
            }

            Conexion conexion = new Conexion();
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Materias (NombreMateria) VALUES (@nombre)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nombre", textBox5.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Materia registrada con éxito.");

                    LimpiarCamposMaterias();
                    CargarMaterias();
                    CargarDatosAsignacion(); 
                    CargarMaterias();          
                    CargarDatosAsignacion();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al registrar: " + ex.Message);
                }
            }
        }
    }
}