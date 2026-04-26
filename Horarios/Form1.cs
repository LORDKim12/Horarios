using MySql.Data.MySqlClient;

namespace Horarios
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Ahora textBox1 será el Nombre y textBox2 será la Matrícula
            string nombreMaestro = textBox1.Text;
            string matriculaMaestro = textBox2.Text;

            Conexion conexion = new Conexion();

            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                try
                {
                    conn.Open();

                    // Modificamos el SELECT para validar contra la tabla Maestros
                    string query = "SELECT COUNT(1) FROM Maestros WHERE Nombre = @nombre AND Matricula = @matricula";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@nombre", nombreMaestro);
                    cmd.Parameters.AddWithValue("@matricula", matriculaMaestro);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("¡Bienvenido, " + nombreMaestro + "!");

                        // Abrir el Menú Principal
                        MenuPrincipal menu = new MenuPrincipal();
                        menu.Show();
                        this.Hide(); // Ocultar el Form de Login
                    }
                    else
                    {
                        // Mensaje de error más específico
                        MessageBox.Show("Nombre o Matrícula incorrectos. Verifica tus datos.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al conectar a la base de datos: " + ex.Message);
                }
            }
        }
    }
}
