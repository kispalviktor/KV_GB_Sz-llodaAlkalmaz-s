
using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SzallodaAlkalmazas
{
    public partial class Form1 : Form
    {
        private readonly string connectionString =
            "Server=localhost;Port=3306;Database=SzallodaDb;Uid=root;Pwd=mysql;CharSet=utf8;";

        private readonly Hotel hotel = new Hotel();
        private readonly ListBox lista = new ListBox();
        private bool mintaBetoltve = false;

        public Form1()
        {
            InitializeComponent();

            Text = "Szállodai foglaláskezelő";
            Size = new Size(900, 550);
            StartPosition = FormStartPosition.CenterScreen;

            Label cim = new Label
            {
                Text = "SZÁLLODAI FOGLALÁSKEZELŐ",
                Font = new Font("Arial", 18, FontStyle.Bold),
                Location = new Point(20, 20),
                Size = new Size(600, 40)
            };

            Button mintaGomb = new Button
            {
                Text = "Mintaadatok betöltése",
                Location = new Point(20, 75),
                Size = new Size(200, 40)
            };

            Button utkozesGomb = new Button
            {
                Text = "Ütközés tesztelése",
                Location = new Point(235, 75),
                Size = new Size(190, 40)
            };

            Button szabadGomb = new Button
            {
                Text = "Szabad szobák keresése",
                Location = new Point(440, 75),
                Size = new Size(210, 40)
            };

            lista.Location = new Point(20, 135);
            lista.Size = new Size(830, 330);
            lista.Font = new Font("Consolas", 10);

            mintaGomb.Click += (s, e) => MintaadatokBetoltese();
            utkozesGomb.Click += (s, e) => UtkozesTesztelese();
            szabadGomb.Click += (s, e) => SzabadSzobakTesztje();

            Controls.Add(cim);
            Controls.Add(mintaGomb);
            Controls.Add(utkozesGomb);
            Controls.Add(szabadGomb);
            Controls.Add(lista);
        }

        private void MintaadatokBetoltese()
        {
            if (mintaBetoltve)
            {
                MessageBox.Show("A mintaadatok már be vannak töltve.");
                return;
            }

            try
            {
                hotel.AddRoom(new Room(
                    101, "Egyágyas", 1, 25000m, "Wi-Fi, TV"));

                hotel.AddRoom(new Room(
                    102, "Kétágyas", 2, 35000m, "Wi-Fi, TV"));

                hotel.AddRoom(new Room(
                    201, "Lakosztály", 4, 65000m, "Minibár, Wi-Fi"));

                hotel.AddGuest(new Guest(
                    1, "Kovács Péter", "+36301234567",
                    "peter@example.com"));

                hotel.AddGuest(new Guest(
                    2, "Nagy Anna", "+36307654321",
                    "anna@example.com"));

                hotel.AddBooking(new Booking(
                    1, 1, 102,
                    new DateTime(2026, 10, 12),
                    new DateTime(2026, 10, 16),
                    2, "Aktív", 35000m));

                mintaBetoltve = true;
                ListaFrissitese();

                MessageBox.Show("A mintaadatok betöltése sikerült!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hiba");
            }
        }

        private void UtkozesTesztelese()
        {
            if (!mintaBetoltve)
            {
                MessageBox.Show("Először töltsd be a mintaadatokat!");
                return;
            }

            try
            {
                hotel.AddBooking(new Booking(
                    2, 2, 102,
                    new DateTime(2026, 10, 14),
                    new DateTime(2026, 10, 18),
                    2, "Aktív", 35000m));

                ListaFrissitese();
                MessageBox.Show("A foglalás létrejött.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ütközésvizsgálat");
            }
        }

        private void SzabadSzobakTesztje()
        {
            if (!mintaBetoltve)
            {
                MessageBox.Show("Először töltsd be a mintaadatokat!");
                return;
            }

            var szobak = hotel.FindAvailableRooms(
                new DateTime(2026, 10, 14),
                new DateTime(2026, 10, 18), 2);

            lista.Items.Clear();
            lista.Items.Add("SZABAD SZOBÁK:");

            foreach (Room szoba in szobak)
                lista.Items.Add(szoba.ToString());

            if (szobak.Count == 0)
                lista.Items.Add("Nincs megfelelő szabad szoba.");
        }

        private void ListaFrissitese()
        {
            lista.Items.Clear();
            lista.Items.Add("=== SZOBÁK ===");

            foreach (Room szoba in hotel.Rooms)
                lista.Items.Add(szoba.ToString());

            lista.Items.Add("");
            lista.Items.Add("=== VENDÉGEK ===");

            foreach (Guest vendeg in hotel.Guests)
                lista.Items.Add(vendeg.ToString());

            lista.Items.Add("");
            lista.Items.Add("=== FOGLALÁSOK ===");

            foreach (Booking foglalas in hotel.Bookings)
                lista.Items.Add(foglalas.ToString());
        }
    }
}
