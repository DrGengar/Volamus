using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Volamus_v1
{
    public static class VertexElementExtractor
    {
        public static Vector3[] GetVertexElement(ModelMeshPart meshPart, VertexElementUsage usage)
        {
            VertexDeclaration vd = meshPart.VertexBuffer.VertexDeclaration;
            VertexElement[] elements = vd.GetVertexElements();

            Func<VertexElement, bool> elementPredicate = ve => ve.VertexElementUsage == usage && ve.VertexElementFormat == VertexElementFormat.Vector3;
            if (!elements.Any(elementPredicate))
                return null;

            VertexElement element = elements.First(elementPredicate);

            Vector3[] vertexData = new Vector3[meshPart.NumVertices];
            meshPart.VertexBuffer.GetData((meshPart.VertexOffset * vd.VertexStride) + element.Offset,
                vertexData, 0, vertexData.Length, vd.VertexStride);

            return vertexData;
        }
    }

    public class BoundingBoxBuffers
    {
        public VertexBuffer Vertices;
        public int VertexCount;
        public IndexBuffer Indices;
        public int PrimitiveCount;
    }

    public class Player
    {
        public Effect effect;

        Vector3 position;
        int max_jump_height, min_jump_height;
        float jump_velocity;
        float movespeed;
        float gamma;
        int points;
        int touch_count;

        SpriteFont points_font;
        SpriteFont points_font2;

        public BoundingBox innerBoundingBox, outerBoundingBox;

        Vector3 scale;

        Camera camera;

        public Model model;
        public Texture2D Texture;

        Texture2D wingTexture;
        Model leftWing;
        Vector3 leftWingPosition;
        Model rightWing;
        Vector3 rightWingPosition;

        float betta;
        float bettaAlt;

        float alphaDolphin;
        float bettaDolphin;

        float hitAngleLeft;
        float hitAngleRight;
        float hitAngleHigh;


        int direction; //1, if Position.Y < 0, -1 if Position.Y > 0
        bool is_jumping = false;
        bool is_falling = false;
        bool can_hit = false;
        bool is_serving = false;

        Player enemy;

        Vector2[] box = new Vector2[2];

        Model pfeil;
        Texture2D arrowTexture, arrowTexture2;

        KeyboardControl keyboard;
        GamePadControl gamepad;
        bool controller;

        PlayerIndex index;

        //Get-Methods

        public int Direction
        {
            get { return direction; }
        }

        public Camera Camera
        {
            get { return camera; }
            set { camera = value; }
        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector3 PositionLeftWing
        {
            get { return leftWingPosition; }
            set { leftWingPosition = value; }
        }

        public Vector3 PositionRightWing
        {
            get { return rightWingPosition; }
            set { rightWingPosition = value; }
        }

        public int Points
        {
            get { return points; }
            set { points = value; }
        }

        public int Touch_Count
        {
            get { return touch_count; }
            set { touch_count = value; }
        }

        public Player Enemy
        {
            get { return enemy; }
            set
            {
                enemy = value;
            }
        }

        public SpriteFont Font
        {
            get { return points_font; }
        }
        public SpriteFont Font2
        {
            get { return points_font2; }
        }

        public Vector2[] Box
        {
            get { return box; }
        }

        public BoundingBox InnerBoundingBox
        {
            get { return innerBoundingBox; }
        }

        public BoundingBox OuterBoundingBox
        {
            get { return outerBoundingBox; }
        }

        public Model Model
        {
            get { return model; }
        }

        public float Gamma
        {
            get { return gamma; }
        }
        public float Alpha
        {
            get { return alphaDolphin; }
        }
        public float Betta
        {
            get { return bettaDolphin; }
        }
        public bool CanHit
        {
            get { return can_hit; }
            set { can_hit = value; }
        }

        public bool IsFalling
        {
            get { return is_falling; }
            set { is_falling = value; }
        }

        public float JumpVelocity
        {
            get { return jump_velocity; }
        }

        public float Movespeed
        {
            get { return movespeed; }
            set { movespeed = value; }
        }

        public float HitAngleHigh
        {
            get { return hitAngleHigh; }
            set { hitAngleHigh = value; }
        }

        public bool IsServing
        {
            get { return is_serving; }
            set { is_serving = value; }
        }

        public bool Control
        {
            get { return controller; }
        }

        public KeyboardControl KeyboardControl
        {
            get { return keyboard; }
        }

        public GamePadControl GamePadControl
        {
            get { return gamepad; }
        }

        private void Construct(Vector3 pos, int ma_j_height, int mi_j_height, float j_velo, float mvp, Field field)
        {
            position = pos;


            if (position.Y < 0)
            {
                direction = 1;
                box[0] = new Vector2(-field.Width / 2, -field.Length / 2);
                box[1] = new Vector2(field.Width / 2, 0);
            }
            else
            {
                direction = -1;
                box[0] = new Vector2(field.Width / 2, field.Length / 2);
                box[1] = new Vector2(-field.Width / 2, 0);
            }


            camera = new Camera(new Vector3(0, direction * (-80), 20), new Vector3(0, 0, 0), new Vector3(0, direction * 1, 1)); //0,-60,20  0,0,0   0,1,1

            max_jump_height = ma_j_height;
            min_jump_height = mi_j_height;
            jump_velocity = j_velo;
            movespeed = mvp;

            points = 0;
            gamma = 0.0f;
            touch_count = 0;
            //bettas für die Laufanimation
            betta = 0.0f;
            bettaAlt = 1.0f;
            //für die Schlaganimation
            hitAngleLeft = 90;  //Rotation nach vorne
            hitAngleRight = 90; //  -"-
            hitAngleHigh = 0;   // Rotation nach oben


            scale = new Vector3(2.9f, 2.9f, 2.9f); //0.025
        }

        public Player()
        {

        }

        public Player(Vector3 pos, int ma_j_height, int mi_j_height, float j_velo, float mvp, Field field)
        {
            Construct(pos, ma_j_height, mi_j_height, j_velo, mvp, field);

            controller = false;

            keyboard = new KeyboardControl();
            keyboard = keyboard.LoadContent();

            keyboard.oldstate = Keyboard.GetState();
        }

        public Player(Vector3 pos, int ma_j_height, int mi_j_height, float j_velo, float mvp, Field field, PlayerIndex i)
        {
            Construct(pos, ma_j_height, mi_j_height, j_velo, mvp, field);

            index = i;
            controller = true;

            gamepad = new GamePadControl(i);
            gamepad = gamepad.LoadContent();

            gamepad.oldstate = GamePad.GetState(i);
        }

        //LoadContent
        public void LoadContent()
        {
            effect = GameStateManager.Instance.Content.Load<Effect>("Effects/shader");

            arrowTexture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/red");
            arrowTexture2 = GameStateManager.Instance.Content.Load<Texture2D>("Textures/green");

            if (direction == 1)
            {
                leftWing = GameStateManager.Instance.Content.Load<Model>("Models/wingLeft"); //PingWingLeft
                rightWing = GameStateManager.Instance.Content.Load<Model>("Models/wingRight");
            }
            else
            {
                rightWing = GameStateManager.Instance.Content.Load<Model>("Models/wingLeft"); //PingWingLeft
                leftWing = GameStateManager.Instance.Content.Load<Model>("Models/wingRight");
            }

            wingTexture = GameStateManager.Instance.Content.Load<Texture2D>("Models/PingWingUV");

            points_font = GameStateManager.Instance.Content.Load<SpriteFont>("SpriteFonts/Standard");
            points_font2 = GameStateManager.Instance.Content.Load<SpriteFont>("SpriteFonts/Headline");

            pfeil = GameStateManager.Instance.Content.Load<Model>("Models/pfeil");
        }

        public void UnloadContent(){}

        //Update
        public void Update(Field field)
        {
            if (controller)
            {
                UpdateController(field, gamepad.Up, gamepad.Down, gamepad.Left, gamepad.Right, gamepad.Jump, gamepad.Weak,
                    gamepad.Strong, gamepad.Dir_Left, gamepad.Dir_Right);
            }
            else
            {
                UpdateKeyboard(field, keyboard.Up, keyboard.Down, keyboard.Left, keyboard.Right, keyboard.Jump, keyboard.Weak,
                    keyboard.Strong, keyboard.Dir_Left, keyboard.Dir_Right);
            }
        }

        //Tastatur Anfang
        private void UpdateKeyboard(Field field, Keys Up, Keys Down, Keys Left, Keys Right, Keys Jump, Keys weak, Keys strong, Keys l, Keys r)
        {
            if (GameScreen.Instance.Match.IsFinished != true)
            {
                // Schlaganimation
                if (is_serving || enemy.is_serving)
                {
                    hitAngleLeft = 90;
                    hitAngleRight = 90;
                    hitAngleHigh = 0;
                }
                if (Ball.Instance.IsFlying)
                {
                    hitAngleLeft = 90;
                    hitAngleRight = 90;
                    hitAngleHigh = 0;
                }
                if (keyboard.IsKeyDown(weak))
                {
                    hitAngleRight = 120;
                    hitAngleLeft = 60;
                    hitAngleHigh = 0;

                }
                if (keyboard.IsKeyDown(strong))
                {
                    hitAngleRight = 120;
                    hitAngleLeft = 60;
                    hitAngleHigh = 0;
                }


                keyboard.newstate = Keyboard.GetState();

                camera.Update();

                //wenn der Spieler steht, werden die Bettas zurückgesetzt
                if (keyboard.IsKeyUp(Up) && keyboard.IsKeyUp(Down) && keyboard.IsKeyUp(Left) && keyboard.IsKeyUp(Right))
                {
                    betta = 0.0f;
                    bettaAlt = 1.0f;
                    alphaDolphin = 0;
                    bettaDolphin = 0;
                }

                //jeder Spieler braucht einen Winkel Gamma, den er verändern kann mit Eingaben
                if (direction == 1)
                {
                    if (keyboard.IsKeyDown(r) && gamma <= 45)
                    {
                        gamma += (direction) * 2.0f; //Ein Grad mehr/weniger
                    }
                    if (keyboard.IsKeyDown(l) && gamma >= -45)
                    {
                        gamma -= (direction) * 2.0f; //Ein Grad mehr/weniger
                    }
                }
                else
                {
                    if (direction == -1)
                    {
                        if (keyboard.IsKeyDown(r) && gamma >= -45)
                        {
                            gamma -= 2.0f; //Ein Grad mehr/weniger
                        }
                        if (keyboard.IsKeyDown(l) && gamma <= 45)
                        {
                            gamma += 2.0f; //Ein Grad mehr/weniger
                        }
                    }
                }

                //Spieler hat gerade Aufschlag -> Position hinten, Bewegung nur links un rechts, Sprung
                if (is_serving)
                {
                    WeakThrow(weak);
                    StrongThrow(strong);

                    float offset = position.Y - box[0].Y;
                    position.Y -= offset;
                    MovingBoundingBoxes(new Vector3(0, -offset, 0));

                    if (!is_jumping)
                    {
                        if (keyboard.IsKeyDown(Left))
                        {
                            //Delfin
                            bettaDolphin = 10;

                            //watscheln
                            if (betta <= 0 && betta >= -5 && betta <= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta--;
                            }
                            if (betta < -5)
                            {
                                betta = -5;
                                bettaAlt = -6;
                            }
                            if (betta <= 0 && betta >= -5 && betta >= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta++;
                            }
                            if (betta <= 5 && betta >= 0 && betta >= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta++;
                            }
                            if (betta > 5)
                            {
                                betta = 5;
                                bettaAlt = 6;
                            }
                            if (betta <= 5 && betta >= 0 && betta <= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta--;
                            }

                            if (direction == 1)
                            {
                                if (position.X > box[0].X) //position.X > box[0].X
                                {
                                    position.X -= movespeed;
                                    MovingBoundingBoxes(new Vector3(-movespeed, 0, 0));

                                    camera.AddPosition(new Vector3(-movespeed, 0, 0));
                                    camera.AddView(new Vector3(-movespeed, 0, 0));
                                }
                            }
                            else if (direction == -1)
                            {
                                if (position.X < box[0].X) //position.X < box[0].X
                                {
                                    position.X += movespeed;
                                    MovingBoundingBoxes(new Vector3(movespeed, 0, 0));

                                    camera.AddPosition(new Vector3(movespeed, 0, 0));
                                    camera.AddView(new Vector3(movespeed, 0, 0));
                                }
                            }
                        }

                        if (keyboard.IsKeyDown(Right))
                        {
                            //Delfin 
                            bettaDolphin = -10;

                            //watscheln
                            if (betta <= 0 && betta >= -5 && betta <= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta--;
                            }
                            if (betta < -5)
                            {
                                betta = -5;
                                bettaAlt = -6;
                            }
                            if (betta <= 0 && betta >= -5 && betta >= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta++;
                            }
                            if (betta <= 5 && betta >= 0 && betta >= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta++;
                            }
                            if (betta > 5)
                            {
                                betta = 5;
                                bettaAlt = 6;
                            }
                            if (betta <= 5 && betta >= 0 && betta <= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta--;
                            }

                            if (direction == 1)
                            {
                                if (position.X < box[1].X) //position.X < box[1].X
                                {
                                    position.X += movespeed;
                                    MovingBoundingBoxes(new Vector3(movespeed, 0, 0));

                                    camera.AddPosition(new Vector3(movespeed, 0, 0));
                                    camera.AddView(new Vector3(movespeed, 0, 0));
                                }
                            }
                            else if (direction == -1)
                            {
                                if (position.X > box[1].X) //position.X > box[1].X
                                {
                                    position.X -= movespeed;
                                    MovingBoundingBoxes(new Vector3(-movespeed, 0, 0));

                                    camera.AddPosition(new Vector3(-movespeed, 0, 0));
                                    camera.AddView(new Vector3(-movespeed, 0, 0));
                                }
                            }
                        }

                        if (keyboard.IsKeyDown(Jump))
                        {
                            is_jumping = true;
                            is_falling = false;
                        }
                    }
                    else
                    {
                        if (position.Z <= max_jump_height && !is_falling)
                        {
                            position.Z += jump_velocity;
                            MovingBoundingBoxes(new Vector3(0, 0, jump_velocity));
                        }
                        else
                        {
                            if (position.Z > min_jump_height)
                            {
                                position.Z -= jump_velocity;
                                MovingBoundingBoxes(new Vector3(0, 0, -jump_velocity));
                                is_falling = true;
                            }
                            else
                            {
                                is_jumping = false;
                            }
                        }
                    }

                    //Setze Ball-Position auf aktuelle Position des Aufschlägers
                    if (direction == 1)
                    {
                        Ball.Instance.Position = new Vector3((OuterBoundingBox.Max.X + OuterBoundingBox.Min.X) / 2, OuterBoundingBox.Max.Y, OuterBoundingBox.Max.Z + Ball.Instance.EffectDrop);
                    }
                    else
                    {
                        if (direction == -1)
                        {
                            Ball.Instance.Position = new Vector3((OuterBoundingBox.Max.X + OuterBoundingBox.Min.X) / 2, OuterBoundingBox.Min.Y, OuterBoundingBox.Max.Z + Ball.Instance.EffectDrop);//+ 1);Ball.Instance.EffectDrop
                        }
                    }
                }
                else //Normales Spielen, alle Bewegungen
                {
                    WeakThrow(weak);
                    StrongThrow(strong);

                    if (!is_jumping)
                    {
                        if (keyboard.IsKeyDown(Up))
                        {
                            //Delfin
                            alphaDolphin = 20;

                            //watscheln
                            if (betta <= 0 && betta >= -5 && betta <= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta--;
                            }
                            if (betta < -5)
                            {
                                betta = -5;
                                bettaAlt = -6;
                            }
                            if (betta <= 0 && betta >= -5 && betta >= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta++;
                            }
                            if (betta <= 5 && betta >= 0 && betta >= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta++;
                            }
                            if (betta > 5)
                            {
                                betta = 5;
                                bettaAlt = 6;
                            }
                            if (betta <= 5 && betta >= 0 && betta <= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta--;
                            }


                            if (direction == 1)
                            {
                                if (!Collision.Instance.PlayerWithNet(this, field)) //Collision Spieler mit Netz
                                {
                                    position.Y += movespeed;
                                    MovingBoundingBoxes(new Vector3(0, movespeed, 0));
                                }
                            }
                            else if (direction == -1)
                            {
                                if (!Collision.Instance.PlayerWithNet(this, field)) //Collision Spieler mit Netz
                                {
                                    position.Y -= movespeed;
                                    MovingBoundingBoxes(new Vector3(0, -movespeed, 0));
                                }
                            }
                        }

                        if (keyboard.IsKeyDown(Left))
                        {
                            //Delfin
                            bettaDolphin = 10;

                            //watscheln
                            if (betta <= 0 && betta >= -5 && betta <= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta--;
                            }
                            if (betta < -5)
                            {
                                betta = -5;
                                bettaAlt = -6;
                            }
                            if (betta <= 0 && betta >= -5 && betta >= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta++;
                            }
                            if (betta <= 5 && betta >= 0 && betta >= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta++;
                            }
                            if (betta > 5)
                            {
                                betta = 5;
                                bettaAlt = 6;
                            }
                            if (betta <= 5 && betta >= 0 && betta <= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta--;
                            }

                            if (direction == 1)
                            {

                                if (position.X > box[0].X) //position.X > box[0].X
                                {
                                    position.X -= movespeed;
                                    MovingBoundingBoxes(new Vector3(-movespeed, 0, 0));

                                    camera.AddPosition(new Vector3(-movespeed, 0, 0));
                                    camera.AddView(new Vector3(-movespeed, 0, 0));
                                }
                            }
                            else if (direction == -1)
                            {
                                if (position.X < box[0].X) //position.X < box[0].X
                                {
                                    position.X += movespeed;
                                    MovingBoundingBoxes(new Vector3(movespeed, 0, 0));

                                    camera.AddPosition(new Vector3(movespeed, 0, 0));
                                    camera.AddView(new Vector3(movespeed, 0, 0));
                                }
                            }
                        }

                        if (keyboard.IsKeyDown(Down))
                        {
                            //Delfin
                            alphaDolphin = -20;

                            //watscheln
                            if (betta <= 0 && betta >= -5 && betta <= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta--;
                            }
                            if (betta < -5)
                            {
                                betta = -5;
                                bettaAlt = -6;
                            }
                            if (betta <= 0 && betta >= -5 && betta >= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta++;
                            }
                            if (betta <= 5 && betta >= 0 && betta >= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta++;
                            }
                            if (betta > 5)
                            {
                                betta = 5;
                                bettaAlt = 6;
                            }
                            if (betta <= 5 && betta >= 0 && betta <= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta--;
                            }

                            if (direction == 1)
                            {
                                if (position.Y > box[0].Y) //position.Y > box[0].Y
                                {
                                    position.Y -= movespeed;
                                    MovingBoundingBoxes(new Vector3(0, -movespeed, 0));
                                }
                            }
                            else if (direction == -1)
                            {
                                if (position.Y < box[0].Y) //position.Y < box[0].Y
                                {
                                    position.Y += movespeed;
                                    MovingBoundingBoxes(new Vector3(0, movespeed, 0));
                                }
                            }
                        }

                        if (keyboard.IsKeyDown(Right))
                        {
                            //Delfin 
                            bettaDolphin = -10;

                            //watscheln
                            if (betta <= 0 && betta >= -5 && betta <= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta--;
                            }
                            if (betta < -5)
                            {
                                betta = -5;
                                bettaAlt = -6;
                            }
                            if (betta <= 0 && betta >= -5 && betta >= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta++;
                            }
                            if (betta <= 5 && betta >= 0 && betta >= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta++;
                            }
                            if (betta > 5)
                            {
                                betta = 5;
                                bettaAlt = 6;
                            }
                            if (betta <= 5 && betta >= 0 && betta <= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta--;
                            }

                            if (direction == 1)
                            {

                                if (position.X < box[1].X) //position.X < box[1].X
                                {
                                    position.X += movespeed;
                                    MovingBoundingBoxes(new Vector3(movespeed, 0, 0));

                                    camera.AddPosition(new Vector3(movespeed, 0, 0));
                                    camera.AddView(new Vector3(movespeed, 0, 0));
                                }
                            }
                            else if (direction == -1)
                            {
                                if (position.X > box[1].X) //position.X > box[1].X
                                {
                                    position.X -= movespeed;
                                    MovingBoundingBoxes(new Vector3(-movespeed, 0, 0));
                                    camera.AddPosition(new Vector3(-movespeed, 0, 0));
                                    camera.AddView(new Vector3(-movespeed, 0, 0));
                                }
                            }
                        }

                        if (keyboard.IsKeyDown(Jump))
                        {
                            is_jumping = true;
                            is_falling = false;
                        }
                    }
                    else
                    {
                        if (position.Z <= max_jump_height && !is_falling)
                        {
                            position.Z += jump_velocity;
                            MovingBoundingBoxes(new Vector3(0, 0, jump_velocity));
                        }
                        else
                        {
                             if (position.Z > min_jump_height)
                            {
                                position.Z -= jump_velocity;
                                MovingBoundingBoxes(new Vector3(0, 0, -jump_velocity));
                                is_falling = true;
                            }
                            else
                            {
                                is_jumping = false;
                            }
                        }
                    }
                }

                if (Touch_Count > 3)
                {
                    Enemy.Points++;
                    Enemy.IsServing = true;
                    IsServing = false;
                    Ball.Instance.Active = null;

                    Touch_Count = 0;
                    Enemy.Touch_Count = 0;
                }

                keyboard.oldstate = keyboard.newstate;
                
                if (direction == 1)
                {
                    leftWingPosition = new Vector3(position.X + 1, position.Y, position.Z -3);
                    rightWingPosition = new Vector3(position.X - 1, position.Y, position.Z -3);
                }
                else
                {
                    leftWingPosition = new Vector3(position.X + 1, position.Y, position.Z -3);
                    rightWingPosition = new Vector3(position.X - 1, position.Y, position.Z -3);
                }
            }
            else
            {
                betta = 0;
                gamma = 0;

                hitAngleLeft = 90;
                hitAngleRight = 90;

                if (GameScreen.Instance.Match.Winner == this)
                {
                    //Flügel gehen nach oben
                    hitAngleHigh = 20;

                    //hüpft
                    if (position.Z < 7 && !is_falling)
                    {
                        position.Z += jump_velocity;
                    }
                    else
                    {
                        if (Position.Z > min_jump_height)
                        {
                            position.Z -= jump_velocity;
                            is_falling = true;
                        }
                        else
                        {
                            is_falling = false;
                        }
                    }

                    leftWingPosition = new Vector3(position.X - 2, position.Y, position.Z - 3);
                    rightWingPosition = new Vector3(position.X + 2, position.Y, position.Z - 3);
                }
                else
                {
                    PositionLeftWing = new Vector3(Position.X + 5, Position.Y, Position.Z + 2);
                    PositionRightWing = new Vector3(Position.X - 5, Position.Y, Position.Z + 2);
                    HitAngleHigh = -40;
                }
            }
        }

        //Schwacher Schlag
        private void WeakThrow(Keys weakthrow)
        {
            bool double_hit;

            if (keyboard.oldstate.IsKeyUp(weakthrow))
            {
                double_hit = false;
            }
            else
            {
                double_hit = true;
            }

            if (Keyboard.GetState().IsKeyDown(weakthrow) && can_hit && !double_hit) //Drückt Knopf und darf schlagen
            {

                //GameStateManager.Instance.Ingame.SoundVolume = 0.4f;
                GameStateManager.Instance.Ingame.Play2D("Content//Sound//Ball.ogg");

                if (is_serving) //Falls man Aufschlag hatte, hat man nach dem Schlagen ihn erstmal nicht mehr (Ball fliegt ja jetzt)
                {
                    is_serving = false;
                }

                if (Collision.Instance.LastTouched.Enemy == this)
                {
                    Collision.Instance.LastTouched.Touch_Count = 0;
                }

                Touch_Count++;

                //Man darf nicht gleich nochmal schlagen, erst wenn der ball wieder die äußere BoundingBox des Spielers berührt
                can_hit = false;

                Collision.Instance.LastTouched = this; //Setze diese Person als die, die den Ball als letztes berührt hat
                Ball.Instance.IsFlying = true; //Ball fliegt

                //neue Parabel und an Ball übergeben
                Parabel weak = new Parabel(Ball.Instance.Position, 50.0f, gamma, 45.0f, 20.0f, direction);
                Ball.Instance.Active = weak;

                //Ball updaten
                Ball.Instance.Wind.Update();
            }
        }

        //Starker Schlag
        private void StrongThrow(Keys strongthrow)
        {
            bool double_hit;

            if (keyboard.oldstate.IsKeyUp(strongthrow))
            {
                double_hit = false;
            }
            else
            {
                double_hit = true;
            }

            if (Keyboard.GetState().IsKeyDown(strongthrow) && can_hit && !double_hit) //Drückt Knopf und darf schlagen
            {
                //GameStateManager.Instance.Ingame.SoundVolume = 1.0f;
                GameStateManager.Instance.Ingame.Play2D("Content//Sound//Ball.ogg");

                if (is_serving) //Falls man Aufschlag hatte, hat man nach dem Schlagen ihn erstmal nicht mehr (Ball fliegt ja jetzt)
                {
                    is_serving = false;
                }

                if (Collision.Instance.LastTouched.Enemy == this)
                {
                    Collision.Instance.LastTouched.Touch_Count = 0;
                }

                Touch_Count++;

                //Man darf nicht gleich nochmal schlagen, erst wenn der ball wieder die äußere BoundingBox des Spielers berührt
                can_hit = false;

                Collision.Instance.LastTouched = this; //Setze diese Person als die, die den Ball als letztes berührt hat
                Ball.Instance.IsFlying = true; //Ball fliegt

                //neue Parabel und an Ball übergeben
                Parabel strong = new Parabel(Ball.Instance.Position, 45.0f, gamma, 45.0f, 25.0f, direction);
                Ball.Instance.Active = strong;

                //Ball updaten
                Ball.Instance.Wind.Update();
            }
        }
        //Tastatur Ende

        //  Controller  Anfang
        private void UpdateController(Field field, Buttons Up, Buttons Down, Buttons Left, Buttons Right, Buttons Jump, Buttons weak, Buttons strong, Buttons l, Buttons r)
        {
            if (GameScreen.Instance.Match.IsFinished == false)
            {
                gamepad.newstate = GamePad.GetState(PlayerIndex.One);
                camera.Update();

                //Schlaganimation
                if (is_serving || enemy.is_serving)
                {
                    hitAngleLeft = 90;
                    hitAngleRight = 90;
                    hitAngleHigh = 0;
                }

                if (Ball.Instance.IsFlying)
                {
                    hitAngleLeft = 90;
                    hitAngleRight = 90;
                    hitAngleHigh = 0;
                }

                if (gamepad.IsButtonDown(weak))
                {
                    hitAngleRight = 120;
                    hitAngleLeft = 60;
                    hitAngleHigh = 0;
                }

                if (gamepad.IsButtonDown(strong))
                {
                    hitAngleRight = 120;
                    hitAngleLeft = 60;
                    hitAngleHigh = 0;
                }

                //wenn der Spieler steht, werden die Bettas zurückgesetzt
                if (gamepad.IsButtonUp(Up) && gamepad.IsButtonUp(Down) && gamepad.IsButtonUp(Left) && gamepad.IsButtonUp(Right))
                {
                    betta = 0.0f;
                    bettaAlt = 1.0f;
                    alphaDolphin = 0;
                    bettaDolphin = 0;
                }

                //jeder Spieler braucht einen Winkel Gamma, den er verändern kann mit Eingaben
                if (direction == 1)
                {
                    if (gamepad.IsButtonDown(r) && gamma <= 45)
                    {
                        gamma += (direction) * 2.0f; //Ein Grad mehr/weniger
                    }
                    if (gamepad.IsButtonDown(l) && gamma >= -45)
                    {
                        gamma -= (direction) * 2.0f; //Ein Grad mehr/weniger
                    }
                }
                else
                {
                    if (direction == -1)
                    {
                        if (gamepad.IsButtonDown(r) && gamma >= -45)
                        {
                            gamma -= 2.0f; //Ein Grad mehr/weniger
                        }
                        if (gamepad.IsButtonDown(l) && gamma <= 45)
                        {
                            gamma += 2.0f; //Ein Grad mehr/weniger
                        }
                    }
                }

                //Spieler hat gerade Aufschlag -> Position hinten, Bewegung nur links un rechts, Sprung
                if (is_serving)
                {
                    WeakThrow(weak);
                    StrongThrow(strong);

                    float offset = position.Y - box[0].Y;
                    position.Y -= offset;
                    MovingBoundingBoxes(new Vector3(0, -offset, 0));

                    if (!is_jumping)
                    {
                        if (gamepad.IsButtonDown(Left))
                        {
                            //Delfin
                            bettaDolphin = 10;

                            //watscheln
                            if (betta <= 0 && betta >= -5 && betta <= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta--;
                            }
                            if (betta < -5)
                            {
                                betta = -5;
                                bettaAlt = -6;
                            }
                            if (betta <= 0 && betta >= -5 && betta >= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta++;
                            }
                            if (betta <= 5 && betta >= 0 && betta >= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta++;
                            }
                            if (betta > 5)
                            {
                                betta = 5;
                                bettaAlt = 6;
                            }
                            if (betta <= 5 && betta >= 0 && betta <= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta--;
                            }

                            if (direction == 1)
                            {
                                if (position.X > box[0].X) //position.X > box[0].X
                                {
                                    position.X -= movespeed;
                                    MovingBoundingBoxes(new Vector3(-movespeed, 0, 0));
                                    camera.AddPosition(new Vector3(-movespeed, 0, 0));
                                    camera.AddView(new Vector3(-movespeed, 0, 0));
                                }
                            }
                            else if (direction == -1)
                            {
                                if (position.X < box[0].X) //position.X < box[0].X
                                {
                                    position.X += movespeed;
                                    MovingBoundingBoxes(new Vector3(movespeed, 0, 0));
                                    camera.AddPosition(new Vector3(movespeed, 0, 0));
                                    camera.AddView(new Vector3(movespeed, 0, 0));
                                }
                            }
                        }

                        if (gamepad.IsButtonDown(Right))
                        {
                            //Delfin 
                            bettaDolphin = -10;

                            //watscheln
                            if (betta <= 0 && betta >= -5 && betta <= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta--;
                            }
                            if (betta < -5)
                            {
                                betta = -5;
                                bettaAlt = -6;
                            }
                            if (betta <= 0 && betta >= -5 && betta >= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta++;
                            }
                            if (betta <= 5 && betta >= 0 && betta >= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta++;
                            }
                            if (betta > 5)
                            {
                                betta = 5;
                                bettaAlt = 6;
                            }
                            if (betta <= 5 && betta >= 0 && betta <= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta--;
                            }

                            if (direction == 1)
                            {
                                if (position.X < box[1].X) //position.X < box[1].X
                                {
                                    position.X += movespeed;
                                    MovingBoundingBoxes(new Vector3(movespeed, 0, 0));
                                    camera.AddPosition(new Vector3(movespeed, 0, 0));
                                    camera.AddView(new Vector3(movespeed, 0, 0));
                                }
                            }
                            else if (direction == -1)
                            {
                                if (position.X > box[1].X) //position.X > box[1].X
                                {
                                    position.X -= movespeed;
                                    MovingBoundingBoxes(new Vector3(-movespeed, 0, 0));
                                    camera.AddPosition(new Vector3(-movespeed, 0, 0));
                                    camera.AddView(new Vector3(-movespeed, 0, 0));
                                }
                            }
                        }

                        if (gamepad.IsButtonDown(Jump))
                        {
                            is_jumping = true;
                            is_falling = false;
                        }
                    }
                    else
                    {
                        if (position.Z <= max_jump_height && !is_falling)
                        {
                            position.Z += jump_velocity;
                            MovingBoundingBoxes(new Vector3(0, 0, jump_velocity));
                        }
                        else
                        {
                            if (position.Z > min_jump_height)
                            {
                                position.Z -= jump_velocity;
                                MovingBoundingBoxes(new Vector3(0, 0, -jump_velocity));
                                is_falling = true;
                            }
                            else
                            {
                                is_jumping = false;
                            }
                        }
                    }

                    //Setze Ball-Position auf aktuelle Position des Aufschlägers
                    if (direction == 1)
                    {
                        Ball.Instance.Position = new Vector3((OuterBoundingBox.Max.X + OuterBoundingBox.Min.X) / 2, OuterBoundingBox.Max.Y, OuterBoundingBox.Max.Z + 1);
                    }
                    else
                    {
                        if (direction == -1)
                        {
                            Ball.Instance.Position = new Vector3((OuterBoundingBox.Max.X + OuterBoundingBox.Min.X) / 2, OuterBoundingBox.Min.Y, OuterBoundingBox.Max.Z + 1);
                        }
                    }
                }
                else //Normales Spielen, alle Bewegungen
                {
                    WeakThrow(weak);
                    StrongThrow(strong);

                    if (!is_jumping)
                    {

                        if (gamepad.IsButtonDown(Up))
                        {
                            //Delfin
                            alphaDolphin = 20;

                            //watscheln
                            if (betta <= 0 && betta >= -5 && betta <= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta--;
                            }
                            if (betta < -5)
                            {
                                betta = -5;
                                bettaAlt = -6;
                            }
                            if (betta <= 0 && betta >= -5 && betta >= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta++;
                            }
                            if (betta <= 5 && betta >= 0 && betta >= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta++;
                            }
                            if (betta > 5)
                            {
                                betta = 5;
                                bettaAlt = 6;
                            }
                            if (betta <= 5 && betta >= 0 && betta <= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta--;
                            }


                            if (direction == 1)
                            {
                                if (!Collision.Instance.PlayerWithNet(this, field)) //Collision Spieler mit Netz
                                {
                                    position.Y += movespeed;

                                    MovingBoundingBoxes(new Vector3(0, movespeed, 0));
                                }
                            }
                            else if (direction == -1)
                            {
                                if (!Collision.Instance.PlayerWithNet(this, field)) //Collision Spieler mit Netz
                                {
                                    position.Y -= movespeed;
                                    MovingBoundingBoxes(new Vector3(0, -movespeed, 0));
                                }
                            }
                        }


                        if (gamepad.IsButtonDown(Left))
                        {
                            //Delfin
                            bettaDolphin = 10;

                            //watscheln
                            if (betta <= 0 && betta >= -5 && betta <= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta--;
                            }
                            if (betta < -5)
                            {
                                betta = -5;
                                bettaAlt = -6;
                            }
                            if (betta <= 0 && betta >= -5 && betta >= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta++;
                            }
                            if (betta <= 5 && betta >= 0 && betta >= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta++;
                            }
                            if (betta > 5)
                            {
                                betta = 5;
                                bettaAlt = 6;
                            }
                            if (betta <= 5 && betta >= 0 && betta <= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta--;
                            }

                            if (direction == 1)
                            {
                                if (position.X > box[0].X) //position.X > box[0].X
                                {
                                    position.X -= movespeed;
                                    MovingBoundingBoxes(new Vector3(-movespeed, 0, 0));
                                    camera.AddPosition(new Vector3(-movespeed, 0, 0));
                                    camera.AddView(new Vector3(-movespeed, 0, 0));
                                }
                            }
                            else if (direction == -1)
                            {
                                if (position.X < box[0].X) //position.X < box[0].X
                                {
                                    position.X += movespeed;
                                    MovingBoundingBoxes(new Vector3(movespeed, 0, 0));
                                    camera.AddPosition(new Vector3(movespeed, 0, 0));
                                    camera.AddView(new Vector3(movespeed, 0, 0));
                                }
                            }
                        }


                        if (gamepad.IsButtonDown(Down))
                        {
                            //Delfin
                            alphaDolphin = -20;

                            //watscheln
                            if (betta <= 0 && betta >= -5 && betta <= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta--;
                            }
                            if (betta < -5)
                            {
                                betta = -5;
                                bettaAlt = -6;
                            }
                            if (betta <= 0 && betta >= -5 && betta >= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta++;
                            }
                            if (betta <= 5 && betta >= 0 && betta >= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta++;
                            }
                            if (betta > 5)
                            {
                                betta = 5;
                                bettaAlt = 6;
                            }
                            if (betta <= 5 && betta >= 0 && betta <= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta--;
                            }

                            if (direction == 1)
                            {
                                if (position.Y > box[0].Y) //position.Y > box[0].Y
                                {
                                    position.Y -= movespeed;
                                    MovingBoundingBoxes(new Vector3(0, -movespeed, 0));
                                }
                            }
                            else if (direction == -1)
                            {
                                if (position.Y < box[0].Y) //position.Y < box[0].Y
                                {
                                    position.Y += movespeed;
                                    MovingBoundingBoxes(new Vector3(0, movespeed, 0));
                                }
                            }
                        }


                        if (gamepad.IsButtonDown(Right))
                        {
                            //Delfin 
                            bettaDolphin = -10;

                            //watscheln
                            if (betta <= 0 && betta >= -5 && betta <= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta--;
                            }
                            if (betta < -5)
                            {
                                betta = -5;
                                bettaAlt = -6;
                            }
                            if (betta <= 0 && betta >= -5 && betta >= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta++;
                            }
                            if (betta <= 5 && betta >= 0 && betta >= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta++;
                            }
                            if (betta > 5)
                            {
                                betta = 5;
                                bettaAlt = 6;
                            }
                            if (betta <= 5 && betta >= 0 && betta <= bettaAlt)
                            {
                                bettaAlt = betta;
                                betta--;
                            }

                            if (direction == 1)
                            {
                                if (position.X < box[1].X) //position.X < box[1].X
                                {
                                    position.X += movespeed;
                                    MovingBoundingBoxes(new Vector3(movespeed, 0, 0));
                                    camera.AddPosition(new Vector3(movespeed, 0, 0));
                                    camera.AddView(new Vector3(movespeed, 0, 0));
                                }
                            }
                            else if (direction == -1)
                            {
                                if (position.X > box[1].X) //position.X > box[1].X
                                {
                                    position.X -= movespeed;
                                    MovingBoundingBoxes(new Vector3(-movespeed, 0, 0));
                                    camera.AddPosition(new Vector3(-movespeed, 0, 0));
                                    camera.AddView(new Vector3(-movespeed, 0, 0));
                                }
                            }
                        }

                        if (gamepad.IsButtonDown(Jump))
                        {
                            is_jumping = true;
                            is_falling = false;
                        }
                    }
                    else
                    {
                        if (position.Z <= max_jump_height && !is_falling)
                        {
                            position.Z += jump_velocity;
                            MovingBoundingBoxes(new Vector3(0, 0, jump_velocity));
                        }
                        else
                        {
                            if (position.Z > min_jump_height)
                            {
                                position.Z -= jump_velocity;
                                MovingBoundingBoxes(new Vector3(0, 0, -jump_velocity));
                                is_falling = true;
                            }
                            else
                            {
                                is_jumping = false;
                            }
                        }
                    }
                }

                if (Touch_Count > 3)
                {
                    Enemy.Points++;
                    Enemy.IsServing = true;
                    IsServing = false;
                    Ball.Instance.Active = null;

                    Touch_Count = 0;
                    Enemy.Touch_Count = 0;
                }

                gamepad.oldstate = gamepad.newstate;

                if (direction == 1)
                {
                    leftWingPosition = new Vector3(position.X+1, position.Y, position.Z -3);
                    rightWingPosition = new Vector3(position.X-1, position.Y, position.Z -3);
                }
                else
                {
                    leftWingPosition = new Vector3(position.X+1, position.Y, position.Z -3);
                    rightWingPosition = new Vector3(position.X-1, position.Y, position.Z -3);
                }
            }
            else
            {
                betta = 0;
                gamma = 0;

                hitAngleLeft = 90;
                hitAngleRight = 90;

                if (GameScreen.Instance.Match.Winner == this)
                {
                    //Flügel gehen nach oben
                    leftWingPosition = new Vector3(position.X - 2, position.Y, position.Z - 3);
                    rightWingPosition = new Vector3(position.X + 2, position.Y, position.Z -3);
                    hitAngleHigh = 20;

                    //hüpft
                    if (position.Z < 7 && !is_falling)
                    {
                        position.Z += jump_velocity;
                    }
                    else
                    {
                        if (Position.Z > min_jump_height)
                        {
                            position.Z -= jump_velocity;
                            is_falling = true;
                        }
                        else
                        {
                            is_falling = false;
                        }
                    }
                }
                else
                {
                    PositionLeftWing = new Vector3(Position.X + 5, Position.Y, Position.Z + 2);
                    PositionRightWing = new Vector3(Position.X - 5, Position.Y, Position.Z + 2);
                    HitAngleHigh = -40;
                }
            }
        }

        //Schwacher Schlag
        private void WeakThrow(Buttons weakthrow)
        {
            bool double_hit;

            if (gamepad.oldstate.IsButtonUp(weakthrow))
            {
                double_hit = false;
            }
            else
            {
                double_hit = true;
            }

            if (gamepad.IsButtonDown(weakthrow) && can_hit && !double_hit) //Drückt Knopf und darf schlagen
            {

                //GameStateManager.Instance.Ingame.SoundVolume = 0.4f;
                GameStateManager.Instance.Ingame.Play2D("Content//Sound//Ball.ogg");

                if (is_serving) //Falls man Aufschlag hatte, hat man nach dem Schlagen ihn erstmal nicht mehr (Ball fliegt ja jetzt)
                {
                    is_serving = false;
                }

                if (Collision.Instance.LastTouched.Enemy == this)
                {
                    Collision.Instance.LastTouched.Touch_Count = 0;
                }

                Touch_Count++;

                //Man darf nicht gleich nochmal schlagen, erst wenn der ball wieder die äußere BoundingBox des Spielers berührt
                can_hit = false;

                Collision.Instance.LastTouched = this; //Setze diese Person als die, die den Ball als letztes berührt hat
                Ball.Instance.IsFlying = true; //Ball fliegt

                //neue Parabel und an Ball übergeben
                Parabel weak = new Parabel(Ball.Instance.Position, 50.0f, gamma, 45.0f, 20.0f, direction);
                Ball.Instance.Active = weak;

                //Ball updaten
                Ball.Instance.Wind.Update();
            }
        }

        //Starker Schlag
        private void StrongThrow(Buttons strongthrow)
        {
            bool double_hit;

            if (gamepad.oldstate.IsButtonUp(strongthrow))
            {
                double_hit = false;
            }
            else
            {
                double_hit = true;
            }

            bool temp = gamepad.IsButtonDown(strongthrow);

            if (gamepad.IsButtonDown(strongthrow) && can_hit && !double_hit) //Drückt Knopf und darf schlagen
            {
                //GameStateManager.Instance.Ingame.SoundVolume = 1.0f;
                GameStateManager.Instance.Ingame.Play2D("Content//Sound//Ball.ogg");

                if (is_serving) //Falls man Aufschlag hatte, hat man nach dem Schlagen ihn erstmal nicht mehr (Ball fliegt ja jetzt)
                {
                    is_serving = false;
                }

                if (Collision.Instance.LastTouched.Enemy == this)
                {
                    Collision.Instance.LastTouched.Touch_Count = 0;
                }

                Touch_Count++;

                //Man darf nicht gleich nochmal schlagen, erst wenn der ball wieder die äußere BoundingBox des Spielers berührt
                can_hit = false;

                Collision.Instance.LastTouched = this; //Setze diese Person als die, die den Ball als letztes berührt hat
                Ball.Instance.IsFlying = true; //Ball fliegt

                //neue Parabel und an Ball übergeben
                Parabel strong = new Parabel(Ball.Instance.Position, 45.0f, gamma, 45.0f, 25.0f, direction);
                Ball.Instance.Active = strong;

                //Ball updaten
                Ball.Instance.Wind.Update();
            }
        }
        // Controller Ende

        public void Draw(Camera camera)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;

                    Matrix World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90 + betta)) * Matrix.CreateRotationZ(MathHelper.ToRadians(direction * 90 + direction * (-gamma))) *
                                   Matrix.CreateScale(scale) * Matrix.CreateTranslation(position);
                    Matrix Projection = camera.ProjectionMatrix;
                    Matrix View = camera.ViewMatrix;
                    Matrix WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(World));

                    effect.Parameters["worldMatrix"].SetValue(World);
                    effect.Parameters["worldInverseTransposeMatrix"].SetValue(WorldInverseTransposeMatrix);
                    effect.Parameters["worldViewProjectionMatrix"].SetValue(World * View * Projection);

                    effect.Parameters["cameraPos"].SetValue(camera.Position);
                    effect.Parameters["globalAmbient"].SetValue(new Vector4(0.2f, 0.2f, 0.2f, 1.0f));
                    effect.Parameters["numLights"].SetValue(4);

                    effect.Parameters["PointLightpos"].SetValue(GameScreen.Instance.Match.LightsPosition);
                    effect.Parameters["PointLightambient"].SetValue(GameScreen.Instance.Match.LightsAmbient);
                    effect.Parameters["PointLightdiffuse"].SetValue(GameScreen.Instance.Match.LightsDiffuse);
                    effect.Parameters["PointLightspecular"].SetValue(GameScreen.Instance.Match.LightsSpecular);
                    effect.Parameters["PointLightradius"].SetValue(GameScreen.Instance.Match.LightsRadius);

                    effect.Parameters["Materialambient"].SetValue(new Vector4(0.2f, 0.2f, 0.2f, 1.0f));
                    effect.Parameters["Materialdiffuse"].SetValue(new Vector4(0.8f, 0.8f, 0.8f, 1.0f));
                    effect.Parameters["Materialspecular"].SetValue(new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
                    effect.Parameters["Materialshininess"].SetValue(32.0f);

                    effect.Parameters["colorMapTexture"].SetValue(Texture);
                }
                mesh.Draw();
            }

            DrawWingLeft(camera, leftWing, leftWingPosition);
            DrawWingRight(camera, rightWing, rightWingPosition);
        }

        // für rechten Flügel
        public void DrawWingRight(Camera camera, Model wing, Vector3 position)
        {
            Matrix[] transforms = new Matrix[wing.Bones.Count];
            wing.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in wing.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;

                    Matrix World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90 + (-direction) * hitAngleHigh + betta)) * Matrix.CreateRotationZ(MathHelper.ToRadians(direction * hitAngleRight + direction * (-gamma))) *
                                   Matrix.CreateScale(scale * 5) * Matrix.CreateTranslation(position);
                    Matrix Projection = camera.ProjectionMatrix;
                    Matrix View = camera.ViewMatrix;
                    Matrix WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(World));

                    effect.Parameters["worldMatrix"].SetValue(World);
                    effect.Parameters["worldInverseTransposeMatrix"].SetValue(WorldInverseTransposeMatrix);
                    effect.Parameters["worldViewProjectionMatrix"].SetValue(World * View * Projection);

                    effect.Parameters["cameraPos"].SetValue(camera.Position);
                    effect.Parameters["globalAmbient"].SetValue(new Vector4(0.2f, 0.2f, 0.2f, 1.0f));
                    effect.Parameters["numLights"].SetValue(4);

                    effect.Parameters["PointLightpos"].SetValue(GameScreen.Instance.Match.LightsPosition);
                    effect.Parameters["PointLightambient"].SetValue(GameScreen.Instance.Match.LightsAmbient);
                    effect.Parameters["PointLightdiffuse"].SetValue(GameScreen.Instance.Match.LightsDiffuse);
                    effect.Parameters["PointLightspecular"].SetValue(GameScreen.Instance.Match.LightsSpecular);
                    effect.Parameters["PointLightradius"].SetValue(GameScreen.Instance.Match.LightsRadius);

                    effect.Parameters["Materialambient"].SetValue(new Vector4(0.2f, 0.2f, 0.2f, 1.0f));
                    effect.Parameters["Materialdiffuse"].SetValue(new Vector4(0.8f, 0.8f, 0.8f, 1.0f));
                    effect.Parameters["Materialspecular"].SetValue(new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
                    effect.Parameters["Materialshininess"].SetValue(32.0f);

                    effect.Parameters["colorMapTexture"].SetValue(wingTexture);
                }
                mesh.Draw();
            }
        }

        // für linken Flügel
        public void DrawWingLeft(Camera camera, Model wing, Vector3 position)
        {
            Matrix[] transforms = new Matrix[wing.Bones.Count];
            wing.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in wing.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;

                    Matrix World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90 - (-direction) * hitAngleHigh + betta)) * Matrix.CreateRotationZ(MathHelper.ToRadians(direction * hitAngleLeft + direction * (-gamma))) *
                                   Matrix.CreateScale(scale * 5) * Matrix.CreateTranslation(position);
                    Matrix Projection = camera.ProjectionMatrix;
                    Matrix View = camera.ViewMatrix;
                    Matrix WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(World));

                    effect.Parameters["worldMatrix"].SetValue(World);
                    effect.Parameters["worldInverseTransposeMatrix"].SetValue(WorldInverseTransposeMatrix);
                    effect.Parameters["worldViewProjectionMatrix"].SetValue(World * View * Projection);

                    effect.Parameters["cameraPos"].SetValue(camera.Position);
                    effect.Parameters["globalAmbient"].SetValue(new Vector4(0.2f, 0.2f, 0.2f, 1.0f));
                    effect.Parameters["numLights"].SetValue(4);

                    effect.Parameters["PointLightpos"].SetValue(GameScreen.Instance.Match.LightsPosition);
                    effect.Parameters["PointLightambient"].SetValue(GameScreen.Instance.Match.LightsAmbient);
                    effect.Parameters["PointLightdiffuse"].SetValue(GameScreen.Instance.Match.LightsDiffuse);
                    effect.Parameters["PointLightspecular"].SetValue(GameScreen.Instance.Match.LightsSpecular);
                    effect.Parameters["PointLightradius"].SetValue(GameScreen.Instance.Match.LightsRadius);

                    effect.Parameters["Materialambient"].SetValue(new Vector4(0.2f, 0.2f, 0.2f, 1.0f));
                    effect.Parameters["Materialdiffuse"].SetValue(new Vector4(0.8f, 0.8f, 0.8f, 1.0f));
                    effect.Parameters["Materialspecular"].SetValue(new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
                    effect.Parameters["Materialshininess"].SetValue(32.0f);

                    effect.Parameters["colorMapTexture"].SetValue(wingTexture);
                }
                mesh.Draw();
            }
        }

        public void DrawArrow(Camera camera)
        {
            float temp = 0.0f;

            if (direction == -1)
            {
                temp = 180.0f;
            }

            Matrix[] transforms = new Matrix[pfeil.Bones.Count];
            pfeil.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in pfeil.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;

                    Matrix World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateRotationZ(MathHelper.ToRadians(temp + (direction) * (-gamma))) *
                                   Matrix.CreateScale(0.03f, 0.04f, 0.01f) * Matrix.CreateTranslation(new Vector3(position.X, position.Y, 0.02f));
                    Matrix Projection = camera.ProjectionMatrix;
                    Matrix View = camera.ViewMatrix;
                    Matrix WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(World));

                    effect.Parameters["worldMatrix"].SetValue(World);
                    effect.Parameters["worldInverseTransposeMatrix"].SetValue(WorldInverseTransposeMatrix);
                    effect.Parameters["worldViewProjectionMatrix"].SetValue(World * View * Projection);

                    effect.Parameters["cameraPos"].SetValue(camera.Position);
                    effect.Parameters["globalAmbient"].SetValue(new Vector4(0.2f, 0.2f, 0.2f, 1.0f));
                    effect.Parameters["numLights"].SetValue(4);

                    effect.Parameters["PointLightpos"].SetValue(GameScreen.Instance.Match.LightsPosition);
                    effect.Parameters["PointLightambient"].SetValue(GameScreen.Instance.Match.LightsAmbient);
                    effect.Parameters["PointLightdiffuse"].SetValue(GameScreen.Instance.Match.LightsDiffuse);
                    effect.Parameters["PointLightspecular"].SetValue(GameScreen.Instance.Match.LightsSpecular);
                    effect.Parameters["PointLightradius"].SetValue(GameScreen.Instance.Match.LightsRadius);

                    effect.Parameters["Materialambient"].SetValue(new Vector4(0.2f, 0.2f, 0.2f, 1.0f));
                    effect.Parameters["Materialdiffuse"].SetValue(new Vector4(0.8f, 0.8f, 0.8f, 1.0f));
                    effect.Parameters["Materialspecular"].SetValue(new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
                    effect.Parameters["Materialshininess"].SetValue(32.0f);

                    if (can_hit)
                    {
                        effect.Parameters["colorMapTexture"].SetValue(arrowTexture2);
                    }
                    else
                    {
                        effect.Parameters["colorMapTexture"].SetValue(arrowTexture);
                    }
                }
                mesh.Draw();
            }
        }

        public void MovingBoundingBoxes(Vector3 offset)
        {
            innerBoundingBox.Min += offset;
            innerBoundingBox.Max += offset;
            
            outerBoundingBox.Min += offset;
            outerBoundingBox.Max += offset;
        }
    }
}
