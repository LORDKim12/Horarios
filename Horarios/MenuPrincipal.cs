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
            // 1. Validación básica para evitar registros vacíos
            if (string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("El Nombre y la Matrícula son obligatorios para registrar a un maestro.");
                return;
            }

            // Nota: Usé ConexionBD basado en nuestros ejemplos anteriores. 
            // Si tu clase se llama solo 'Conexion', cámbialo en esta línea.
            Conexion conexion = new Conexion();
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Maestros (Nombre, Matricula, Cedula, Numero) VALUES (@nom, @mat, @ced, @num)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    // Asignar los valores de los TextBox 
                    cmd.Parameters.AddWithValue("@nom", textBox3.Text); // Nombre
                    cmd.Parameters.AddWithValue("@mat", textBox1.Text); // Matrícula
                    cmd.Parameters.AddWithValue("@ced", textBox2.Text); // Cédula
                    cmd.Parameters.AddWithValue("@num", textBox4.Text); // Número

                    cmd.ExecuteNonQuery(); // Ejecuta el INSERT

                    MessageBox.Show("Maestro registrado con éxito.");

                    // Limpiamos los TextBox
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    textBox4.Clear();

                    // --- AQUÍ ESTÁ LA ACTUALIZACIÓN CLAVE ---
                    // Llamamos a todos los métodos de carga para que el nuevo maestro
                    // aparezca inmediatamente en todas las pestañas de tu sistema.
                    CargarMaestros();             // Actualiza la tabla de esta misma pestaña (Maestros)
                    CargarMaestrosParaCombo();    // Actualiza el ComboBox de la pestaña "Horarios"
                    CargarDatosAsignacion();      // Actualiza el ComboBox de la pestaña "Clases"

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

                    // IMPORTANTE: Limpiar el DataSource antes de reasignar
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

        private void dataGridViewMaestros_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificamos que se haya hecho clic en una fila válida (no en los encabezados)
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridViewMaestros.Rows[e.RowIndex];

                // Guardamos el ID internamente (asumiendo que en tu SELECT lo llamaste "ID")
                idMaestroSeleccionado = Convert.ToInt32(fila.Cells["ID"].Value);

                // Pasamos los datos a los TextBox
                textBox3.Text = fila.Cells["Nombre"].Value.ToString();
                textBox1.Text = fila.Cells["Matricula"].Value.ToString();
                textBox2.Text = fila.Cells["Cedula"].Value.ToString();
                textBox4.Text = fila.Cells["Numero"].Value.ToString();
            }
        }
        private int idMaestroSeleccionado = 0;

        // Método para limpiar los cuadros de texto
        private void LimpiarCamposMaestros()
        {
            textBox3.Clear(); // Nombre
            textBox1.Clear(); // Matrícula
            textBox2.Clear(); // Cédula
            textBox4.Clear(); // Número
            idMaestroSeleccionado = 0; // Reiniciamos el ID
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Validar que haya un maestro seleccionado
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
                    CargarMaestros(); // Recargamos la tabla para ver los cambios
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

            // Pedimos confirmación antes de borrar
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
                        // Error 1451 significa que hay una restricción de llave foránea (Foreign Key)
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

                    // 1. Cargar Maestros en el ComboBox
                    string queryMaestros = "SELECT IdMaestro, Nombre FROM Maestros";
                    MySqlDataAdapter adapterMaestros = new MySqlDataAdapter(queryMaestros, conn);
                    DataTable dtMaestros = new DataTable();
                    adapterMaestros.Fill(dtMaestros);

                    comboBoxMaestrosAsignacion.DataSource = dtMaestros;
                    comboBoxMaestrosAsignacion.DisplayMember = "Nombre";
                    comboBoxMaestrosAsignacion.ValueMember = "IdMaestro";
                    comboBoxMaestrosAsignacion.SelectedIndex = -1; // Para que inicie vacío

                    // 2. Cargar Materias en el CheckedListBox
                    string queryMaterias = "SELECT IdMateria, NombreMateria FROM Materias";
                    MySqlDataAdapter adapterMaterias = new MySqlDataAdapter(queryMaterias, conn);
                    DataTable dtMaterias = new DataTable();
                    adapterMaterias.Fill(dtMaterias);

                    // Configuramos el CheckedListBox para que use la base de datos
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
            // 1. Primero, quitamos las palomitas de todo por si había otro maestro seleccionado antes
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
                    // Consultamos qué materias tiene este maestro
                    string query = "SELECT IdMateria FROM Maestros_Materias WHERE IdMaestro = @idMaestro";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idMaestro", idMaestroSeleccionado);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Leemos todos los IDs de las materias que imparte
                        while (reader.Read())
                        {
                            int idMateriaQueImparte = Convert.ToInt32(reader["IdMateria"]);

                            // Buscamos esa materia en el CheckedListBox y le ponemos la palomita
                            for (int i = 0; i < checkedListBoxMaterias.Items.Count; i++)
                            {
                                DataRowView filaItem = (DataRowView)checkedListBoxMaterias.Items[i];
                                int idMateriaLista = Convert.ToInt32(filaItem["IdMateria"]);

                                if (idMateriaLista == idMateriaQueImparte)
                                {
                                    checkedListBoxMaterias.SetItemChecked(i, true);
                                    break; // Encontramos la materia, pasamos a la siguiente del maestro
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

                    // 1. Borramos TODAS las asignaciones actuales de este maestro
                    string queryDelete = "DELETE FROM Maestros_Materias WHERE IdMaestro = @idMaestro";
                    MySqlCommand cmdDelete = new MySqlCommand(queryDelete, conn);
                    cmdDelete.Parameters.AddWithValue("@idMaestro", idMaestro);
                    cmdDelete.ExecuteNonQuery();

                    // 2. Insertamos las materias que están "palomeadas" actualmente
                    string queryInsert = "INSERT INTO Maestros_Materias (IdMaestro, IdMateria) VALUES (@idMaestro, @idMateria)";
                    MySqlCommand cmdInsert = new MySqlCommand(queryInsert, conn);

                    // Recorremos solo los items que están seleccionados (CheckedItems)
                    foreach (object itemChecked in checkedListBoxMaterias.CheckedItems)
                    {
                        // Como usamos un DataSource, el item es un DataRowView
                        DataRowView filaItem = (DataRowView)itemChecked;
                        int idMateria = Convert.ToInt32(filaItem["IdMateria"]);

                        cmdInsert.Parameters.Clear(); // Limpiamos los parámetros del ciclo anterior
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
            // Validamos que el clic sea en una fila con datos (y no en los encabezados grises)
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridViewMaterias.Rows[e.RowIndex];

                // Guardamos el ID de la materia seleccionada
                idMateriaSeleccionada = Convert.ToInt32(fila.Cells["ID"].Value);

                // Pasamos el texto a los TextBox
                textBox5.Text = fila.Cells["Nombre"].Value.ToString();

                // *Nota: Si agregaste la columna "Matricula" o "Clave" a tu tabla Materias en MySQL,
                // también la puedes pasar así:
                // textBoxMatriculaMateria.Text = fila.Cells["Matricula"].Value.ToString();
            }
        }

        private int idMateriaSeleccionada = 0;

        private void LimpiarCamposMaterias()
        {
            // Asumiendo estos nombres de TextBox según tu diseño de "Clases"
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
                    // Seleccionamos los datos para el DataGridView de materias
                    string query = "SELECT IdMateria AS ID, NombreMateria AS Nombre FROM Materias";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridViewMaterias.DataSource = dt; // El DataGridView que pusiste en la pestaña Clases
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
                    // Actualizamos el nombre. Si usas matrícula, agrégala también a la consulta.
                    string query = "UPDATE Materias SET NombreMateria = @nom WHERE IdMateria = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@nom", textBox5.Text);
                    cmd.Parameters.AddWithValue("@id", idMateriaSeleccionada);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Materia modificada exitosamente.");

                    LimpiarCamposMaterias();
                    CargarMaterias(); // Tu método para volver a llenar el DataGridView
                    CargarDatosAsignacion(); // Si hiciste la parte del CheckedListBox, esto la actualiza
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
                        CargarMaterias(); // Refrescar la tabla
                    }
                    catch (MySqlException ex)
                    {
                        // Error 1451: Llave foránea. Significa que la materia está en uso.
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
                    CargarDatosAsignacion(); // Actualiza el CheckedListBox de la otra sección
                    CargarMaterias();          // Actualiza la tabla de la pestaña Clases
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