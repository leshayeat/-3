using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace HeroTraining
{
    public partial class MainForm : Form
    {
        // Характеристики героя
        private int strength = 5;
        private int agility = 5;
        private int gold = 50;
        private int energy = 30;

        // Элементы интерфейса
        private Label lblTitle;
        private Label lblStrength;
        private Label lblAgility;
        private Label lblGold;
        private Label lblEnergy;
        private Button btnTrain;
        private Button btnWork;
        private Button btnFight;
        private ComboBox cmbShop;
        private Label lblItemPrice;
        private Button btnBuy;
        private Timer energyTimer;

        // Данные магазина
        private Dictionary<string, Item> shopItems;

        public MainForm()
        {
            InitializeComponent();
            SetupShopItems();
            SetupForm();
            UpdateStats();
        }

        private void SetupShopItems()
        {
            shopItems = new Dictionary<string, Item>
            {
                { "Гантели (+2 Силы, цена: 20)", new Item { StrengthBonus = 2, AgilityBonus = 0, Price = 20 } },
                { "Витамины (+2 Ловкости, цена: 20)", new Item { StrengthBonus = 0, AgilityBonus = 2, Price = 20 } },
                { "Протеин (+3 Силы, цена: 35)", new Item { StrengthBonus = 3, AgilityBonus = 0, Price = 35 } },
                { "Энергетик (+3 Ловкости, цена: 35)", new Item { StrengthBonus = 0, AgilityBonus = 3, Price = 35 } }
            };
        }

        private void SetupForm()
        {
            // Форма
            this.Text = "Тренировка героя";
            this.Size = new Size(450, 520);
            this.BackColor = Color.FromArgb(30, 30, 50);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Заголовок
            lblTitle = new Label();
            lblTitle.Text = "🏋️ ТРЕНИРОВОЧНЫЙ ЗАЛ";
            lblTitle.Font = new Font("Arial", 14, FontStyle.Bold);
            lblTitle.ForeColor = Color.Gold;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(80, 15);
            this.Controls.Add(lblTitle);

            // Статистика
            int statY = 60;
            int spacing = 30;

            lblStrength = CreateStatLabel($"💪 Сила: {strength}", new Point(30, statY), Color.Cyan);
            lblAgility = CreateStatLabel($"🎯 Ловкость: {agility}", new Point(30, statY + spacing), Color.LightGreen);
            lblGold = CreateStatLabel($"💰 Золото: {gold}", new Point(30, statY + spacing * 2), Color.Gold);
            lblEnergy = CreateStatLabel($"⚡ Энергия: {energy}", new Point(30, statY + spacing * 3), Color.Orange);

            // Кнопка тренировки
            btnTrain = new Button();
            btnTrain.Text = "🏃 Тренироваться (+1 Силы)";
            btnTrain.Font = new Font("Arial", 10, FontStyle.Bold);
            btnTrain.Size = new Size(250, 35);
            btnTrain.Location = new Point(30, statY + spacing * 4 + 10);
            btnTrain.BackColor = Color.SteelBlue;
            btnTrain.ForeColor = Color.White;
            btnTrain.FlatStyle = FlatStyle.Flat;
            btnTrain.Cursor = Cursors.Hand;
            btnTrain.Click += BtnTrain_Click;
            this.Controls.Add(btnTrain);

            // Кнопка работы
            btnWork = new Button();
            btnWork.Text = "💼 Работать (+10 Золота)";
            btnWork.Font = new Font("Arial", 10, FontStyle.Bold);
            btnWork.Size = new Size(250, 35);
            btnWork.Location = new Point(30, statY + spacing * 4 + 50);
            btnWork.BackColor = Color.DarkGoldenrod;
            btnWork.ForeColor = Color.White;
            btnWork.FlatStyle = FlatStyle.Flat;
            btnWork.Cursor = Cursors.Hand;
            btnWork.Click += BtnWork_Click;
            this.Controls.Add(btnWork);

            // Магазин
            Label lblShop = new Label();
            lblShop.Text = "🛒 МАГАЗИН:";
            lblShop.Font = new Font("Arial", 10, FontStyle.Bold);
            lblShop.ForeColor = Color.Plum;
            lblShop.AutoSize = true;
            lblShop.Location = new Point(30, statY + spacing * 4 + 100);
            this.Controls.Add(lblShop);

            cmbShop = new ComboBox();
            cmbShop.Font = new Font("Arial", 9);
            cmbShop.Size = new Size(280, 25);
            cmbShop.Location = new Point(30, statY + spacing * 4 + 125);
            cmbShop.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbShop.SelectedIndexChanged += CmbShop_SelectedIndexChanged;
            foreach (string itemName in shopItems.Keys)
            {
                cmbShop.Items.Add(itemName);
            }
            cmbShop.SelectedIndex = 0;
            this.Controls.Add(cmbShop);

            // Цена и кнопка купить
            lblItemPrice = new Label();
            lblItemPrice.Text = "💰 Цена: 20";
            lblItemPrice.Font = new Font("Arial", 10, FontStyle.Bold);
            lblItemPrice.ForeColor = Color.Gold;
            lblItemPrice.AutoSize = true;
            lblItemPrice.Location = new Point(320, statY + spacing * 4 + 128);
            this.Controls.Add(lblItemPrice);

            btnBuy = new Button();
            btnBuy.Text = "Купить";
            btnBuy.Font = new Font("Arial", 9, FontStyle.Bold);
            btnBuy.Size = new Size(80, 30);
            btnBuy.Location = new Point(320, statY + spacing * 4 + 155);
            btnBuy.BackColor = Color.MediumSeaGreen;
            btnBuy.ForeColor = Color.White;
            btnBuy.FlatStyle = FlatStyle.Flat;
            btnBuy.Cursor = Cursors.Hand;
            btnBuy.Click += BtnBuy_Click;
            this.Controls.Add(btnBuy);

            // Кнопка в бой
            btnFight = new Button();
            btnFight.Text = "⚔️ В БОЙ!";
            btnFight.Font = new Font("Arial", 14, FontStyle.Bold);
            btnFight.Size = new Size(370, 50);
            btnFight.Location = new Point(30, statY + spacing * 4 + 200);
            btnFight.BackColor = Color.Crimson;
            btnFight.ForeColor = Color.White;
            btnFight.FlatStyle = FlatStyle.Flat;
            btnFight.Cursor = Cursors.Hand;
            btnFight.Click += BtnFight_Click;
            this.Controls.Add(btnFight);

            // Таймер энергии
            energyTimer = new Timer();
            energyTimer.Interval = 1000; // 1 секунда
            energyTimer.Tick += EnergyTimer_Tick;
            energyTimer.Start();
        }

        private Label CreateStatLabel(string text, Point location, Color color)
        {
            Label label = new Label();
            label.Text = text;
            label.Font = new Font("Arial", 12, FontStyle.Bold);
            label.ForeColor = color;
            label.AutoSize = true;
            label.Location = location;
            this.Controls.Add(label);
            return label;
        }

        private void UpdateStats()
        {
            lblStrength.Text = $"💪 Сила: {strength}";
            lblAgility.Text = $"🎯 Ловкость: {agility}";
            lblGold.Text = $"💰 Золото: {gold}";
            lblEnergy.Text = $"⚡ Энергия: {energy}";

            // Блокируем кнопки, если нет энергии
            btnTrain.Enabled = energy > 0;
            btnWork.Enabled = energy > 0;
            btnBuy.Enabled = energy > 0;
        }

        private void CmbShop_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Динамическое обновление цены при выборе предмета
            if (cmbShop.SelectedItem != null)
            {
                string selectedItem = cmbShop.SelectedItem.ToString();
                if (shopItems.ContainsKey(selectedItem))
                {
                    lblItemPrice.Text = $"💰 Цена: {shopItems[selectedItem].Price}";
                }
            }
        }

        private void BtnTrain_Click(object sender, EventArgs e)
        {
            strength += 1;
            UpdateStats();
        }

        private void BtnWork_Click(object sender, EventArgs e)
        {
            gold += 10;
            UpdateStats();
        }

        private void BtnBuy_Click(object sender, EventArgs e)
        {
            if (cmbShop.SelectedItem == null) return;

            string selectedItem = cmbShop.SelectedItem.ToString();
            if (!shopItems.ContainsKey(selectedItem)) return;

            Item item = shopItems[selectedItem];

            // Валидация: хватает ли золота
            if (gold < item.Price)
            {
                MessageBox.Show(
                    "Недостаточно золота!",
                    "Ошибка покупки",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            // Покупаем
            gold -= item.Price;
            strength += item.StrengthBonus;
            agility += item.AgilityBonus;

            MessageBox.Show(
                $"Куплено: {selectedItem.Split('(')[0].Trim()}\n+{item.StrengthBonus} Силы, +{item.AgilityBonus} Ловкости",
                "Покупка успешна",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            UpdateStats();
        }

        private void BtnFight_Click(object sender, EventArgs e)
        {
            // Открываем арену и передаём характеристики
            energyTimer.Stop();
            this.Hide();

            BattleForm battleForm = new BattleForm(strength, agility, gold);
            battleForm.FormClosed += (s, args) =>
            {
                // Получаем результат боя
                if (battleForm.BossDefeated)
                {
                    gold = battleForm.GoldReward;
                    MessageBox.Show(
                        "🎉 Босс повержен! Вы получаете 100 золота!",
                        "Победа!",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation
                    );
                }
                energyTimer.Start();
                UpdateStats();
                this.Show();
            };
            battleForm.Show();
        }

        private void EnergyTimer_Tick(object sender, EventArgs e)
        {
            if (energy > 0)
            {
                energy -= 1;
                UpdateStats();
            }
            else
            {
                energyTimer.Stop();
            }
        }
    }

    // Класс предмета магазина
    public class Item
    {
        public int StrengthBonus { get; set; }
        public int AgilityBonus { get; set; }
        public int Price { get; set; }
    }
}
