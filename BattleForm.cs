using System;
using System.Drawing;
using System.Windows.Forms;

namespace HeroTraining
{
    public partial class BattleForm : Form
    {
        // Характеристики
        private int heroStrength;
        private int heroAgility;
        private int goldReward;

        // Босс
        private int bossHealth = 100;
        private Random rnd = new Random();

        // Флаг победы
        public bool BossDefeated { get; private set; } = false;
        public int GoldReward => goldReward;

        // Элементы интерфейса
        private Label lblTitle;
        private Label lblBossHealth;
        private Label lblHeroStats;
        private Label lblBattleLog;
        private Button btnAttack;
        private PictureBox picHero;
        private PictureBox picBoss;
        private Panel heroPanel;
        private Panel bossPanel;

        public BattleForm(int strength, int agility, int gold)
        {
            InitializeComponent();
            heroStrength = strength;
            heroAgility = agility;
            goldReward = gold;
            SetupForm();
        }

        private void SetupForm()
        {
            this.Text = "⚔️ Арена — Битва с Боссом";
            this.Size = new Size(650, 500);
            this.BackColor = Color.FromArgb(40, 0, 0);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Заголовок
            lblTitle = new Label();
            lblTitle.Text = "⚔️ БИТВА С БОССОМ ⚔️";
            lblTitle.Font = new Font("Arial", 16, FontStyle.Bold);
            lblTitle.ForeColor = Color.Red;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(150, 10);
            this.Controls.Add(lblTitle);

            // Панель героя
            heroPanel = new Panel();
            heroPanel.Size = new Size(80, 120);
            heroPanel.Location = new Point(100, 60);
            heroPanel.BackColor = Color.FromArgb(0, 100, 200);
            this.Controls.Add(heroPanel);

            Label lblHeroName = new Label();
            lblHeroName.Text = "ГЕРОЙ";
            lblHeroName.Font = new Font("Arial", 10, FontStyle.Bold);
            lblHeroName.ForeColor = Color.Cyan;
            lblHeroName.AutoSize = true;
            lblHeroName.Location = new Point(115, 50);
            this.Controls.Add(lblHeroName);

            picHero = new PictureBox();
            picHero.Size = new Size(60, 90);
            picHero.Location = new Point(10, 15);
            picHero.BackColor = Color.Cyan;
            // Рисуем простую фигуру героя
            picHero.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.FillRectangle(new SolidBrush(Color.Cyan), 0, 0, 60, 90);
                g.DrawString("⚔️", new Font("Arial", 30), Brushes.Black, 10, 25);
            };
            heroPanel.Controls.Add(picHero);

            // Панель босса
            bossPanel = new Panel();
            bossPanel.Size = new Size(80, 120);
            bossPanel.Location = new Point(430, 60);
            bossPanel.BackColor = Color.FromArgb(200, 0, 0);
            this.Controls.Add(bossPanel);

            Label lblBossName = new Label();
            lblBossName.Text = "БОСС";
            lblBossName.Font = new Font("Arial", 10, FontStyle.Bold);
            lblBossName.ForeColor = Color.Red;
            lblBossName.AutoSize = true;
            lblBossName.Location = new Point(450, 50);
            this.Controls.Add(lblBossName);

            picBoss = new PictureBox();
            picBoss.Size = new Size(60, 90);
            picBoss.Location = new Point(10, 15);
            picBoss.BackColor = Color.Red;
            // Рисуем простую фигуру босса
            picBoss.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.FillRectangle(new SolidBrush(Color.Red), 0, 0, 60, 90);
                g.DrawString("👹", new Font("Arial", 30), Brushes.Black, 10, 25);
            };
            bossPanel.Controls.Add(picBoss);

            // Здоровье босса
            lblBossHealth = new Label();
            lblBossHealth.Text = $"❤️ Здоровье Босса: {bossHealth} / 100";
            lblBossHealth.Font = new Font("Arial", 14, FontStyle.Bold);
            lblBossHealth.ForeColor = Color.OrangeRed;
            lblBossHealth.AutoSize = true;
            lblBossHealth.Location = new Point(180, 200);
            this.Controls.Add(lblBossHealth);

            // Статистика героя
            lblHeroStats = new Label();
            lblHeroStats.Text = $"💪 Сила: {heroStrength} | 🎯 Ловкость: {heroAgility} | 💰 Золото: {goldReward}";
            lblHeroStats.Font = new Font("Arial", 10, FontStyle.Bold);
            lblHeroStats.ForeColor = Color.LightGray;
            lblHeroStats.AutoSize = true;
            lblHeroStats.Location = new Point(120, 230);
            this.Controls.Add(lblHeroStats);

            // Лог боя
            lblBattleLog = new Label();
            lblBattleLog.Text = "Нажми 'Ударить', чтобы начать бой!";
            lblBattleLog.Font = new Font("Arial", 10);
            lblBattleLog.ForeColor = Color.Yellow;
            lblBattleLog.AutoSize = true;
            lblBattleLog.MaximumSize = new Size(500, 0);
            lblBattleLog.Location = new Point(120, 260);
            this.Controls.Add(lblBattleLog);

            // Кнопка удара
            btnAttack = new Button();
            btnAttack.Text = "⚡ УДАРИТЬ!";
            btnAttack.Font = new Font("Arial", 14, FontStyle.Bold);
            btnAttack.Size = new Size(400, 60);
            btnAttack.Location = new Point(100, 350);
            btnAttack.BackColor = Color.DarkRed;
            btnAttack.ForeColor = Color.White;
            btnAttack.FlatStyle = FlatStyle.Flat;
            btnAttack.Cursor = Cursors.Hand;
            btnAttack.Click += BtnAttack_Click;
            this.Controls.Add(btnAttack);
        }

        private void BtnAttack_Click(object sender, EventArgs e)
        {
            if (bossHealth <= 0) return;

            // Расчёт урона
            int baseDamage = heroStrength * 2;

            // Критический удар (шанс = ловкость / 2 %)
            int critChance = heroAgility / 2;
            if (critChance > 90) critChance = 90;

            int roll = rnd.Next(1, 101);
            bool isCrit = roll <= critChance;

            int damage = isCrit ? baseDamage * 2 : baseDamage;

            // Наносим урон
            bossHealth -= damage;
            if (bossHealth < 0) bossHealth = 0;

            // Обновляем здоровье босса
            lblBossHealth.Text = $"❤️ Здоровье Босса: {bossHealth} / 100";

            // Лог боя
            string hitType = isCrit ? "💥 КРИТИЧЕСКИЙ УДАР!" : "⚔️ Обычный удар";
            lblBattleLog.Text = $"{hitType}\nНанесено урона: {damage}\n(Шанс крита: {critChance}%, выпало: {roll})";

            // Анимация (меняем цвет панели босса при ударе)
            Color originalColor = bossPanel.BackColor;
            bossPanel.BackColor = Color.Yellow;
            Timer flashTimer = new Timer();
            flashTimer.Interval = 100;
            flashTimer.Tick += (s, args) =>
            {
                bossPanel.BackColor = originalColor;
                flashTimer.Stop();
            };
            flashTimer.Start();

            // Проверка победы
            if (bossHealth <= 0)
            {
                BossDefeated = true;
                goldReward += 100;
                btnAttack.Enabled = false;
                lblBattleLog.Text = "🎉 ПОБЕДА! Босс повержен!\n+100 золота!";
                lblBattleLog.ForeColor = Color.Gold;

                MessageBox.Show(
                    "🎉 Поздравляем! Вы победили босса!\n\nНаграда: +100 золота\n\nОкно закроется автоматически.",
                    "Победа!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation
                );

                this.Close();
            }
        }
    }
}
