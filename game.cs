/*
  by: jaracoder.com
*/

using System;

public class SdlMuncher
{
    static int countTime;
    static int countEnemy;
    static int totallives = 3;
    static int pointeat;
    static int amountOfDots;
    static int countsecuence;
    static int totalsecuence;
    static int amountBigDots;
    static int startX = 32 * 12;
    static int startY = 32 * 8;
    static int lives;
    static int x, y;
    static int pacSpeed;
    static int amountOfEnemies;
    static int countImageR;
    static int countImageL;
    static int countImageU;
    static int countImageD;
    static int score;

    static bool MovePacmanRight;
    static bool MovePacmanLeft;
    static bool MovePacmanTop;
    static bool MovePacmanDown;
    static bool secuencepacdeath;
    static bool stopenemies;
    static bool pursued;
    static bool sessionFinished = false;
    static bool FruitVisible;
    static bool gameFinished;

    static Random randomGenerator;

    struct Dot
    {
        public int x;
        public int y;
        public bool visible;
    }
    struct BigDot
    {
        public int x;
        public int y;
        public bool visible;
    }
    struct Enemy
    {
        public float x;
        public float y;
        public float xSpeed;
        public float ySpeed;
        public bool visible;
        public bool pursued;
    }

    static Dot[] dots;
    static BigDot[] Bigdots;
    static Enemy[] enemies;

    static Font fontJoystix18, fontJoystix10;

    static Image banner;
    static Image dotImage;
    static Image dotBigImage;
    static Image logoDevelopedBy;
    static Image enemyImage;
    static Image enemyImageRed;
    static Image enemyImagePurple;
    static Image enemyImageBlue;
    static Image enemyPursued;
    static Image Fruit;
    static Image pacImageR;
    static Image pacImageR1;
    static Image pacImageL;
    static Image pacImageL1;
    static Image pacImageD;
    static Image pacImageD1;
    static Image pacImageU;
    static Image pacImageU1;
    static Image wallImage;

    static Sonido eatPac;
    static Sonido pacdead;
    static Sonido pacbegin;
    static Sonido pacghost;

    static string[] map = {
            "                       ",
            "    -----------------   ",
            "    -o......-......o-   ",
            "    -.--.--.-.--.--.-   ",
            "    -...............-   ",
            "    -.--.-.---.-.--.-   ",
            "    -....-..-..-....-   ",
            "    ----.--...--.----   ",
            "    ........ ........   ",
            "    ----.-.-.-.-.----   ",
            "    -....-.-.-.-....-   ",
            "    -.--.-.---.-.--.-   ",
            "    -...............-   ",
            "    -.--.-.---.-.--.-   ",
            "    -o...-..p..-...o-   ",
            "    -----------------   "
    };

    static string[] pacdeath = {"data/pacD01.png", "data/pacD01.png", "data/pacD01.png", "data/pacD01.png", "data/pacD01.png",
                                "data/pacD02.png", "data/pacD02.png", "data/pacD02.png", "data/pacD02.png", "data/pacD02.png",
                                "data/pacD03.png", "data/pacD03.png", "data/pacD03.png", "data/pacD03.png", "data/pacD03.png",
                                "data/pacD04.png", "data/pacD04.png", "data/pacD04.png", "data/pacD04.png", "data/pacD04.png",
                                "data/pacD05.png", "data/pacD05.png", "data/pacD05.png", "data/pacD05.png", "data/pacD05.png",
                                "data/pacD06.png", "data/pacD06.png", "data/pacD06.png", "data/pacD06.png", "data/pacD06.png",};

    public static void Init()
    {
        bool fullScreen = false;
        Console.Title = "Pac-Man";
        
        SdlHardware.InitWindow(800, 600, 24, fullScreen, "Pac-Man", "data/logo.bmp");

        pointeat = 0;
        countTime = 0;
        countEnemy = 0;
        countsecuence = 0;
        countImageR=0;
        countImageL=0;
        countImageU=0;
        countImageD=0;
        
        totalsecuence = pacdeath.Length;
        secuencepacdeath = false;
        banner = new Image("data/banner.png");
        dotImage = new Image("data/dot.png");
        dotBigImage = new Image("data/bigDot.png");
        enemyImage = new Image("data/ghostGreen.png");
        enemyImageRed = new Image("data/ghostRed.png");
        enemyImagePurple = new Image("data/ghostPurple.png");
        enemyImageBlue = new Image("data/ghostBlue.png");
        enemyPursued = new Image("data/ghostGrey.png");
        pacImageR = new Image("data/pac01r.png");
        pacImageL = new Image("data/pac01l.png");
        pacImageD = new Image("data/pac01d.png");
        pacImageU = new Image("data/pac01u.png");
        pacImageR1 = new Image("data/pac02r.png");
        pacImageL1 = new Image("data/pac02l.png");
        pacImageD1 = new Image("data/pac02d.png");
        pacImageU1 = new Image("data/pac02u.png");
        wallImage = new Image("data/wall.png");
        logoDevelopedBy = new Image("data/developed_by.png");

        eatPac = new Sonido("data/paceat.wav");
        pacghost = new Sonido("data/pacghost.wav");
        pacbegin = new Sonido("data/pacbegin.wav");
        pacdead = new Sonido("data/pacdead.wav");

        fontJoystix18 = new Font("data/Joystix.ttf", 18);
        fontJoystix10 = new Font("data/Joystix.ttf", 10);

        // Data for the dots
        // First: count how many dots are there
        amountOfDots = 0;
        amountBigDots = 0;
        for (int row = 0; row < 16; row++)
        {
            for (int column = 0; column < 23; column++)
            {
                if (map[row][column] == '.')
                    amountOfDots++;
                if (map[row][column] == 'o')
                    amountBigDots++;
            }
        }
        dots = new Dot[amountOfDots];
        Bigdots = new BigDot[amountBigDots];

        // Now, assign their coordinates
        int currentDot = 0;
        int currentBigDot = 0;
        for (int row = 0; row < 16; row++)
        {
            for (int column = 0; column < 23; column++)
            {
                if (map[row][column] == '.')
                {
                    dots[currentDot].x = column * 32;
                    dots[currentDot].y = row * 32;
                    currentDot++;
                }
                if (map[row][column] == 'o')
                {
                    Bigdots[currentBigDot].x = column * 32;
                    Bigdots[currentBigDot].y = row * 32;
                    currentBigDot++;
                }
            }
        }

        // And enemies
        amountOfEnemies = 4;
        enemies = new Enemy[amountOfEnemies];

        randomGenerator = new Random();

        switch (randomGenerator.Next(0, 4))
        {
            case 0:
                Fruit = new Image("data/apple.png");
                break;
            case 1:
                Fruit = new Image("data/cherry.png");
                break;
            case 2:
                Fruit = new Image("data/cookie.png");
                break;
            case 3:
                Fruit = new Image("data/strawberry.png");
                break;
        }

        FruitVisible = true;
        
      
    }

    public static void PrepareGameStart()
    {
        // Pac coordinates and speed
        pointeat = 0;
        x = startX;
        y = startY;
        pacSpeed = 4;
        countImageR = 0;
        countImageL = 0;
        countImageU = 0;
        countImageD = 0;

        EnemysNotPursued();
        countsecuence = 0;
        countTime = 0;
        secuencepacdeath = false;
        stopenemies = false;
        pursued = false;

        MovePacmanLeft = false;
        MovePacmanRight = false;
        MovePacmanDown = false;
        MovePacmanTop = false;
        // Coordinates for the enemies        
        enemies[0].x = 32*5; enemies[0].y = 2 * 32; enemies[0].xSpeed = 4;
        enemies[1].x = 32*19; enemies[1].y = 2 * 32; enemies[1].xSpeed = 4;
        enemies[2].x = 32*19; enemies[2].y = 14 * 32; enemies[2].xSpeed = -4;
        enemies[3].x = 5*32; enemies[3].y = 14 * 32; enemies[3].xSpeed = 4;

        for (int i = 0; i < amountOfEnemies; i++)
            enemies[i].visible = true;

        // All dots must be visible
        for (int i = 0; i < amountOfDots; i++)
            dots[i].visible = true;

        // All Bigdots must be visible
        for (int i = 0; i < amountBigDots; i++)
             Bigdots[i].visible = true;

        // Resto of data for a new game
        score = 0;
        totallives = 3;

        switch (randomGenerator.Next(0, 4))
        {
            case 0:
                Fruit = new Image("data/apple.png");
                break;
            case 1:
                Fruit = new Image("data/cherry.png");
                break;
            case 2:
                Fruit = new Image("data/cookie.png");
                break;
            case 3:
                Fruit = new Image("data/strawberry.png");
                break;
        }
        FruitVisible = true;
    }

    public static void Intro()
    {
        int x = -40;
        bool exitIntro = false;
        pacbegin.Reproducir1();
        int countimagepac = 0;
        do
        {
            SdlHardware.ClearScreen();
            SdlHardware.DrawHiddenImage(banner, 40, 50);
            SdlHardware.WriteHiddenText("Press Enter to Start", 260, 380, 0xFF, 0xFF, 0xFF, fontJoystix18);
            SdlHardware.WriteHiddenText("(H) Help", 315, 420, 0xFF, 0xFF, 0xFF, fontJoystix18);
            SdlHardware.WriteHiddenText("(C) Credits", 315, 450, 0xFF, 0xFF, 0xFF, fontJoystix18);
            SdlHardware.WriteHiddenText("(Q) Quit", 315, 480, 0xFF, 0xFF, 0xFF, fontJoystix18);
            
            SdlHardware.DrawHiddenImage(enemyImage, x - 100, 300);
            SdlHardware.DrawHiddenImage(enemyImageBlue, x - 150, 300);
            SdlHardware.DrawHiddenImage(enemyImageRed, x - 200, 300);
            SdlHardware.DrawHiddenImage(enemyImagePurple, x - 250, 300);

            SdlHardware.WriteHiddenText("Ver. 1.0.0.1", 30, 575, 0xFF, 0xFF, 0xFF, fontJoystix10);
            SdlHardware.WriteHiddenText("developed by", 650, 558, 0xFF, 0xFF, 0xFF, fontJoystix10);
            SdlHardware.DrawHiddenImage(logoDevelopedBy, 648, 568);

            if (countimagepac == 0)
            {
                SdlHardware.DrawHiddenImage(pacImageR, x, 300);
                countimagepac++;
            }
            else
            {
                SdlHardware.DrawHiddenImage(pacImageR1, x, 300);
                countimagepac = 0;
            }

            SdlHardware.ShowHiddenScreen();
           
            x += 8;
            if (x > 1200)
            {
                x = -40;
            }
              
            SdlHardware.Pause(20);

            if (SdlHardware.KeyPressed(SdlHardware.KEY_C))
            {
                ShowCredits();
            }
            else if (SdlHardware.KeyPressed(SdlHardware.KEY_H))
            {
                ShowHelp();
            }
            else if (SdlHardware.KeyPressed(SdlHardware.KEY_RETURN))
            {
                exitIntro = true;
            }
            else if (SdlHardware.KeyPressed(SdlHardware.KEY_Q))
            {
                exitIntro = true;
                sessionFinished = true;
            }
        }
        while (!exitIntro);
    }

    public static void ShowCredits()
    {
        SdlHardware.ClearScreen();
        SdlHardware.WriteHiddenText("By DAM Ies San Vicente 2012-2013",
            200, 500,
            0xCC, 0xCC, 0xCC,
            fontJoystix18);
        SdlHardware.WriteHiddenText("Hit ESC to return",
            300, 540,
            0x99, 0x99, 0x99,
            fontJoystix18);
        SdlHardware.ShowHiddenScreen();
        do
        {
            SdlHardware.Pause(20);
        }
        while (!SdlHardware.KeyPressed(SdlHardware.KEY_ESC));
    }

    public static void ShowHelp()
    {
        SdlHardware.ClearScreen();
        SdlHardware.WriteHiddenText("Eat the dots, avoid the ghosts.",
            200, 500,
            0xCC, 0xCC, 0xCC,
            fontJoystix18);
        SdlHardware.WriteHiddenText("Hit ESC to return",
            300, 540,
            0x99, 0x99, 0x99,
            fontJoystix18);
        SdlHardware.ShowHiddenScreen();
        do
        {
            SdlHardware.Pause(20);
        }
        while (!SdlHardware.KeyPressed(SdlHardware.KEY_ESC));
    }

    public static bool CanMoveTo(int x, int y, string[] map)
    {
        bool canMove = true;
        for (int row = 0; row < 16; row++)
        {
            for (int column = 0; column < 23; column++)
            {
                if (map[row][column] == '-')
                    if ((x > column * 32 - 32) &&
                        (x < column * 32 + 32) &&
                        (y > row * 32 - 32) &&
                        (y < row * 32 + 32)
                        )
                    {
                        canMove = false;
                    }
            }
        }
        return canMove;
    }

    public static void DrawElements()
    {
        // Draw
        SdlHardware.ClearScreen();
        //Console.Write("Score: {0}",score);

        if (!FruitVisible)
            SdlHardware.DrawHiddenImage(Fruit, 32 * 8, 540);

        // Background map
        for (int row = 0; row < 16; row++)
        {
            for (int column = 0; column < 23; column++)
            {
                if ((map[row][column] == 'p') && (FruitVisible))
                    SdlHardware.DrawHiddenImage(Fruit, column * 32, row * 32);
                if (map[row][column] == '-')
                    SdlHardware.DrawHiddenImage(wallImage, column * 32, row * 32);
            }
        }

        for (int i = 0; i < amountOfDots; i++)
            if (dots[i].visible)
                SdlHardware.DrawHiddenImage(dotImage, dots[i].x, dots[i].y);

        for (int i = 0; i < amountBigDots; i++)
            if (Bigdots[i].visible)
                SdlHardware.DrawHiddenImage(dotBigImage, Bigdots[i].x, Bigdots[i].y);

        if ((MovePacmanLeft) && (!secuencepacdeath))
        {
            if (countImageL == 0)
            {
                SdlHardware.DrawHiddenImage(pacImageL, x, y);
                countImageL++;
            }
            else
            {
                SdlHardware.DrawHiddenImage(pacImageL1, x, y);
                countImageL = 0;
            }
               
            if (CanMoveTo(x - pacSpeed, y, map))
               x -= pacSpeed;
            if ((x == 32 * 4) && (y == 32 * 8))
                x = 32 * 20;  
        }
        else if( (MovePacmanRight) && (!secuencepacdeath))
        {
            if (countImageR == 0)
            {
                SdlHardware.DrawHiddenImage(pacImageR, x, y);
                countImageR++;
            }
            else
            {
                SdlHardware.DrawHiddenImage(pacImageR1, x, y);
                countImageR = 0;
            }
            if (CanMoveTo(x + pacSpeed, y, map))
                x += pacSpeed;
            if ((x == 32 * 20) && (y == 32 * 8))
                x = 32 * 4;   
        }
        else if ((MovePacmanTop) && (!secuencepacdeath))
        {
            if (countImageU == 0)
            {
                SdlHardware.DrawHiddenImage(pacImageU, x, y);
                countImageU++;
            }
            else
            {
                SdlHardware.DrawHiddenImage(pacImageU1, x, y);
                countImageU = 0;
            }
            if (CanMoveTo(x, y - pacSpeed, map))
                y -= pacSpeed;
        }
        else if( (MovePacmanDown) && (!secuencepacdeath))
        {
            if (countImageD == 0)
            {
                SdlHardware.DrawHiddenImage(pacImageD, x, y);
                countImageD++;
            }
            else
            {
                SdlHardware.DrawHiddenImage(pacImageD1, x, y);
                countImageD = 0;
            }
            if (CanMoveTo(x, y + pacSpeed, map))
                y += pacSpeed;
        }
        else
            if(!secuencepacdeath)
            SdlHardware.DrawHiddenImage(pacImageR, x, y);
       
         
        for (int i = 0; i < amountOfEnemies; i++)
        {
            if ((enemies[i].visible) && (!enemies[i].pursued))
            {
                switch(i)
                {
                    case 0:
                        SdlHardware.DrawHiddenImage(enemyImage,
                                 (int)enemies[i].x, (int)enemies[i].y);
                        break;
                    case 1:
                        SdlHardware.DrawHiddenImage(enemyImageBlue,
                                 (int)enemies[i].x, (int)enemies[i].y);
                        break;
                    case 2:
                        SdlHardware.DrawHiddenImage(enemyImagePurple,
                                 (int)enemies[i].x, (int)enemies[i].y);
                        break;
                    case 3:
                        SdlHardware.DrawHiddenImage(enemyImageRed,
                                 (int)enemies[i].x, (int)enemies[i].y);
                        break;
                }           
            }
            else if ((enemies[i].pursued) && (enemies[i].visible) ) 
            {
                SdlHardware.DrawHiddenImage(enemyPursued,
                    (int)enemies[i].x, (int)enemies[i].y);
            }

            else 
            {
                if (CheckTimeOfEnemy())
                {
                    switch (randomGenerator.Next(0, 4))
                    {
                        case 0:
                            enemies[i].x = 32 * 5;
                            enemies[i].y = 2 * 32;
                            break;
                        case 1:
                            enemies[i].x = 32 * 19;
                            enemies[i].y = 2 * 32;
                            break;
                        case 2:
                            enemies[i].x = 32 * 19;
                            enemies[i].y = 14 * 32;
                            break;
                        case 3:
                            enemies[i].x = 32 * 5;
                            enemies[i].y = 14 * 32;
                            break;
                    }
                    enemies[i].visible = true;
                    enemies[i].pursued = false;
                    countEnemy = 0;
                }
            }    
        }

       // SdlHardware.WriteHiddenText("Merry Christmas, Daddy!", 32*4, 6, 0xFF, 0xFF, 0xFF,
         //   sans18);
        SdlHardware.WriteHiddenText("Score: " + score,
            32*16, 550,
            0xFF, 0xFF, 0xFF,
            fontJoystix18);

        int ancho = 32 * 4;
        for (int i = 0; i < totallives; i++)
        {
            SdlHardware.DrawHiddenImage(pacImageR, ancho, 540);
            ancho += 32;
        }


       // SdlHardware.WriteHiddenText("Lives: " + lives,
        //    32*4, 540,
         //   0x80, 0x80, 0xFF,
          //  sans18);

        SdlHardware.ShowHiddenScreen();
    }

    public static void CheckInputDevices()
    {
        // Read keys and calculate new position
        if (SdlHardware.KeyPressed(SdlHardware.KEY_RIGHT)
                && CanMoveTo(x + pacSpeed, y, map))
        {
            MovePacmanRight = true;
            MovePacmanLeft = false;
            MovePacmanTop = false;
            MovePacmanDown = false;
        }
            
            // x += pacSpeed;

        if (SdlHardware.KeyPressed(SdlHardware.KEY_LEFT)
                && CanMoveTo(x - pacSpeed, y, map))
        {
            MovePacmanRight = false;
            MovePacmanLeft = true;
            MovePacmanTop = false;
            MovePacmanDown = false;
        }
            // x -= pacSpeed;

        if (SdlHardware.KeyPressed(SdlHardware.KEY_DOWN)
                && CanMoveTo(x, y + pacSpeed, map))
        {
            MovePacmanRight = false;
            MovePacmanLeft = false;
            MovePacmanTop = false;
            MovePacmanDown = true;
        } 
     //   y += pacSpeed;

        if (SdlHardware.KeyPressed(SdlHardware.KEY_UP)
                && CanMoveTo(x, y - pacSpeed, map))
        {
            MovePacmanRight = false;
            MovePacmanLeft = false;
            MovePacmanTop = true;
            MovePacmanDown = false;
        }
            //     y -= pacSpeed;

        if (SdlHardware.KeyPressed(SdlHardware.KEY_ESC))
            gameFinished = true;
    }
    
    public static void MoveElements()
    {
        // Move enemies and environment
        if (!stopenemies)
        {
            for (int i = 0; i < amountOfEnemies; i++)
            {
            //32 *4 y 34 *8
          
                if (CanMoveTo((int)(enemies[i].x + enemies[i].xSpeed),
                    (int)(enemies[i].y + enemies[i].ySpeed), map))
                {
              
                    enemies[i].x += enemies[i].xSpeed;
                    enemies[i].y += enemies[i].ySpeed;
                }
                else
                {
                    switch (randomGenerator.Next(0, 4))
                    {
                        case 0: // Next move: to the right
                            enemies[i].xSpeed = 4;
                            enemies[i].ySpeed = 0;
                            break;
                        case 1: // Next move: to the left
                            enemies[i].xSpeed = -4;
                            enemies[i].ySpeed = 0;
                            break;
                        case 2: // Next move: upwards
                            enemies[i].xSpeed = 0;
                            enemies[i].ySpeed = -4;
                            break;
                        case 3: // Next move: downwards
                            enemies[i].xSpeed = 0;
                            enemies[i].ySpeed = 4;
                            break;
                    }
                }
            }
        }
    }
    
    public static void CheckCollisions()
    {
        // Collisions, lose energy or lives, etc
        for (int i = 0; i < amountOfDots; i++)
            if (dots[i].visible &&
                (x > dots[i].x - 32) &&
                (x < dots[i].x + 32) &&
                (y > dots[i].y - 32) &&
                (y < dots[i].y + 32)
                )
            {
                eatPac.Reproducir1();
                score += 10;
                dots[i].visible = false;
                pointeat++;
            }

        if (FruitVisible &&
               (x > 384 - 32) &&
               (x < 384 + 32) &&
               (y > 448  - 32) &&
               (y < 448 + 32)
               )
        {
            pacghost.Reproducir1();
            score += 50;
            FruitVisible = false;
        }

        for (int i = 0; i < amountBigDots; i++)
            if (Bigdots[i].visible &&
                (x > Bigdots[i].x - 32) &&
                (x < Bigdots[i].x + 32) &&
                (y > Bigdots[i].y - 32) &&
                (y < Bigdots[i].y + 32)
                )
            {
                pacghost.Reproducir1();
                score += 50;
                Bigdots[i].visible = false;
                EnemysPursued();
                pursued = true;
                countTime = 0;
            }

        for (int i = 0; i < amountOfEnemies; i++)
        {
            if ((enemies[i].visible) && (!enemies[i].pursued) &&
                (x > enemies[i].x - 32) &&
                (x < enemies[i].x + 32) &&
                (y > enemies[i].y - 32) &&
                (y < enemies[i].y + 32))
            {
                StopEnemies();
                secuencepacdeath = true;
            }
            if (enemies[i].visible && enemies[i].pursued &&
                (x > enemies[i].x - 32) &&
                (x < enemies[i].x + 32) &&
                (y > enemies[i].y - 32) &&
                (y < enemies[i].y + 32))
            {
                pacghost.Reproducir1();
                score += 200;
                enemies[i].visible = false;
            }     
         }
        


         if ((totallives == 0) || (pointeat == (amountOfDots)))
            gameFinished = true;
    
        if (secuencepacdeath)
            CargarSecuencePacDeath();  
        if (pursued)
            CheckTimeOfPursued();
    }
    
    public static void EnemysPursued() 
    {
        enemies[0].pursued = true;
        enemies[1].pursued = true;
        enemies[2].pursued = true;
        enemies[3].pursued = true;
    }
    
    public static void EnemysNotPursued()
    {
        enemies[0].pursued = false;
        enemies[1].pursued = false;
        enemies[2].pursued = false;
        enemies[3].pursued = false;
    }
    
    public static void CargarSecuencePacDeath() 
    {
        if (countsecuence < totalsecuence)
        {
            if (countsecuence == 0)
                pacdead.Reproducir1();
            Image ImageSecuence = new Image(pacdeath[countsecuence]);
            SdlHardware.DrawHiddenImage(ImageSecuence, x, y);
            SdlHardware.ShowHiddenScreen();
            countsecuence++;
        }
        if (countsecuence == totalsecuence)
        {
            secuencepacdeath = false;
            countsecuence = 0;
            MovePacmanLeft = false;
            MovePacmanRight = false;
            MovePacmanDown = false;
            MovePacmanTop = false;
            x = startX;
            y = startY;
            EnemysNotPursued();
            InitEnemies();
            stopenemies = false;
            totallives--;
        }
    }

    public static void InitEnemies()
    {
        enemies[0].x = 32 * 5; enemies[0].y = 2 * 32; enemies[0].xSpeed = 4; enemies[0].visible = true;
        enemies[1].x = 32 * 19; enemies[1].y = 2 * 32; enemies[1].xSpeed = 4; enemies[1].visible = true;
        enemies[2].x = 32 * 19; enemies[2].y = 14 * 32; enemies[2].xSpeed = -4; enemies[2].visible = true;
        enemies[3].x = 5 * 32; enemies[3].y = 14 * 32; enemies[3].xSpeed = 4; enemies[3].visible = true;
    }

    public static void StopEnemies() 
    {
        stopenemies = true;
    }
    
    public static bool CheckTimeOfEnemy()
    {
        countEnemy++;
        if (countEnemy >= 95)
            return true;
        else
            return false;
    }
  
    public static void CheckTimeOfPursued()
    {
        countTime++;
       

        if (countTime >= 130)
        {
            pursued = false;
            EnemysNotPursued();
          
        }
            
    }
    
    public static void PauseTillNextFrame()
    {
        SdlHardware.Pause(40);
    }

    public static void Main()
    {
        Init();
        Intro();
        while (!sessionFinished)
        {
            PrepareGameStart();

            // Game Loop
            gameFinished = false;
           
            while (!gameFinished)
            {
                DrawElements();
                CheckInputDevices();
                MoveElements();
                CheckCollisions();
                PauseTillNextFrame();
            } 

            Intro();

        } 
    }
}