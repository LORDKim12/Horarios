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

                    dataGridViewMaestros.DataSource = dt; // Llenamos el Grid
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar maestros: " + ex.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Conexion conexion = new Conexion();
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Maestros (Nombre, Matricula, Cedula, Numero) VALUES (@nom, @mat, @ced, @num)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    // Asignar los valores de los TextBox (Asegúrate de que correspondan a tu diseño)
                    cmd.Parameters.AddWithValue("@nom", textBox3.Text); // Nombre
                    cmd.Parameters.AddWithValue("@mat", textBox1.Text); // Matrícula
                    cmd.Parameters.AddWithValue("@ced", textBox2.Text); // Cédula
                    cmd.Parameters.AddWithValue("@num", textBox4.Text); // Número

                    cmd.ExecuteNonQuery(); // Ejecuta el INSERT

                    MessageBox.Show("Maestro registrado con éxito.");

                    // Limpiamos los TextBox
                    textBox1.Clear(); textBox2.Clear(); textBox3.Clear(); textBox4.Clear();

                    // Actualizamos la tabla
                    CargarMaestros();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar: " + ex.Message);
                }
            }
        }

        private void comboBoxMaestro_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // 1. Verificamos que realmente se haya seleccionado un maestro
            if (comboBoxMaestro.SelectedValue != null)
            {
                // Extraemos el ID del maestro seleccionado
                int idMaestroSeleccionado = Convert.ToInt32(comboBoxMaestro.SelectedValue);

                // 2. Cargamos las materias de ese maestro
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
                    // Hacemos un JOIN para traer solo las materias vinculadas a este maestro
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

                    // 3. Llenamos el ComboBox de Materias
                    comboBoxMateria.DataSource = dtMaterias;
                    comboBoxMateria.DisplayMember = "NombreMateria"; // Lo que ve el usuario
                    comboBoxMateria.ValueMember = "IdMateria";       // El valor interno que guardamos

                    // Opcional: Dejarlo en blanco para que el usuario tenga que seleccionar
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
            // 1. Limpiamos por si hay algo previo
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            // 2. Configuraciones de comportamiento del Grid
            dataGridView1.AllowUserToAddRows = false;    // Evita que salga una fila vacía al final
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;               // Solo lectura (el usuario asignará con el botón, no escribiendo)
            dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect; // Permite seleccionar celda por celda
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Que las columnas abarquen todo el ancho
            dataGridView1.RowHeadersVisible = false;     // Oculta la columna gris fea de la izquierda

            // 3. Crear las Columnas (Los Días)
            dataGridView1.Columns.Add("Hora", "Hora");
            dataGridView1.Columns.Add("Lunes", "Lunes");
            dataGridView1.Columns.Add("Martes", "Martes");
            dataGridView1.Columns.Add("Miercoles", "Miércoles");
            dataGridView1.Columns.Add("Jueves", "Jueves");
            dataGridView1.Columns.Add("Viernes", "Viernes");

            // Darle estilo a la columna de la "Hora" para que parezca un encabezado
            dataGridView1.Columns["Hora"].DefaultCellStyle.BackColor = Color.LightGray;
            dataGridView1.Columns["Hora"].DefaultCellStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);
            dataGridView1.Columns["Hora"].Frozen = true; // Para que no se mueva si haces scroll horizontal

            // 4. Crear las Filas (Los Bloques de Horas)
            // Modifica estos horarios según los bloques que maneje tu escuela
            string[] bloquesDeHora = {
        "07:00 - 08:00",
        "08:00 - 09:00",
        "09:00 - 10:00",
        "10:00 - 11:00",
        "11:00 - 12:00",
        "12:00 - 13:00",
        "13:00 - 14:00"
    };

            // Agregamos cada hora como una nueva fila en el DataGridView
            foreach (string hora in bloquesDeHora)
            {
                // El primer valor es la hora, los demás son celdas en blanco para los días
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

                    // comboBox1 es el de Grupos según tu diseño
                    comboBox1.DataSource = dt;
                    comboBox1.DisplayMember = "NombreGrupo";
                    comboBox1.ValueMember = "IdGrupo";
                    comboBox1.SelectedIndex = -1; // Para que aparezca vacío al inicio
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

                    // comboBox3 es el de Maestros según tu diseño
                    comboBoxMaestro.DataSource = dt;
                    comboBoxMaestro.DisplayMember = "Nombre";
                    comboBoxMaestro.ValueMember = "IdMaestro";
                    comboBoxMaestro.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar maestros: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 1. Validaciones básicas
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

            // 2. Extraer IDs de la pantalla
            int idGrupo = Convert.ToInt32(comboBox1.SelectedValue);
            int idMateria = Convert.ToInt32(comboBoxMateria.SelectedValue);
            int idMaestro = Convert.ToInt32(comboBoxMaestro.SelectedValue);

            // El ID del día es el índice de la columna (1 = Lunes, 5 = Viernes)
            int idDia = dataGridView1.CurrentCell.ColumnIndex;
            // El ID de la hora es la fila seleccionada + 1
            int idHora = dataGridView1.CurrentCell.RowIndex + 1;

            // 3. Guardar en Base de Datos
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

                    // 4. Mostrar en pantalla
                    dataGridView1.CurrentCell.Value = $"{comboBoxMateria.Text}\n({comboBoxMaestro.Text})";
                    MessageBox.Show("Horario asignado exitosamente.");
                }
                catch (MySqlException ex)
                {
                    // El código 1062 es cuando se viola una regla UNIQUE (Ej: El maestro ya tiene clase)
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
                            dataGridView1.CurrentCell.Value = ""; // Limpiamos la celda visualmente
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
    }
}