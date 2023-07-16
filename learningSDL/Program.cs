using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Schema;
using SDL2;
using static SDL2.SDL;

namespace TesingAndLearning
{
    class Program
    {
        public struct POS {
            public int RELATIVE_POS_X;
            public int RELATIVE_POS_Y;
            public int START_POS_X;
            public int START_POS_Y;
        }
        public struct buffer
        {
            public buffer(float x, float y)
            {
                this.x_pos = x;
                this.y_pos = y;
            }
            public float y_pos;
            public float x_pos;
        }


        static int Main(String[] args) {



            var window = IntPtr.Zero;
            var renderer = IntPtr.Zero;
            var playerTexture = IntPtr.Zero;
            var bulletTexture = IntPtr.Zero;
            int SCREEN_WIDTH = 800;
            int SCREEN_HEIGHT = 600;
            float TARGET_FPS = 1000 / 144;

            SDL_Init(SDL_INIT_VIDEO);

            SDL_CreateWindowAndRenderer(SCREEN_WIDTH, SCREEN_HEIGHT, 0,out window,out renderer);

            playerTexture = SDL_CreateTexture(renderer, SDL_PIXELFORMAT_RGBA8888,
                (int)SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET, 100, 100);
            bulletTexture = SDL_CreateTextureFromSurface(renderer, SDL_image.IMG_Load("bullet.png"));

            SDL_SetWindowTitle(window, "SDL LEARNING");
            SDL_SetRenderDrawColor(renderer, 255, 0,0,255);
            SDL_RenderClear(renderer);

            POS posPlayer;
            posPlayer.RELATIVE_POS_Y = 0;
            posPlayer.RELATIVE_POS_X = 0;
            buffer PlayerBuff = new buffer(0,0);
            buffer BullertBuffer = new buffer(0, 0);

            SDL_Rect rect;
            rect.h = 100;
            rect.w = 100;
            rect.x = posPlayer.START_POS_X = 5;
            rect.y = posPlayer.START_POS_Y = 5;
             float y_pos = 0;
             float x_pos = 0;


            SDL_Rect rectBullet;
            rectBullet.h = 20;
            rectBullet.w = 20;
            rectBullet.x = posPlayer.RELATIVE_POS_X;
            rectBullet.y = posPlayer.RELATIVE_POS_Y;
            float y_posB = 0;
            float x_posB = 0;

            SDL_Rect Area;
            Area.h = SCREEN_HEIGHT;
            Area.w = SCREEN_WIDTH;
            Area.x = 0;
            Area.y = 0;

            //SDL_Rect intersection;

            //SDL_IntersectRect(ref rect, ref rect1, out intersection);
            //SDL_SetRenderDrawColor(renderer, 0,0,255,255);
            //SDL_RenderFillRect(renderer, ref intersection);
            SDL_Event events;
            IntPtr keyboardState;
            int arrayKeys;
            //Main loop
            bool loop = !false;
            bool pause = false;
            UInt64 oldTime = SDL_GetTicks();
            UInt64 newTime;
            float deltaTime = 0;
            while (loop)
            {
                newTime = SDL_GetTicks();
                deltaTime = (newTime - oldTime);
                rectBullet.x = posPlayer.RELATIVE_POS_X + 50;
                rectBullet.y = posPlayer.RELATIVE_POS_Y + 50;

                keyboardState = SDL_GetKeyboardState(out arrayKeys);


                //Making the screen red
                SDL_SetRenderDrawColor(renderer, 255, 0, 0, 255);
                SDL_RenderClear(renderer);

                //We give the texture a white color
                SDL_SetRenderTarget(renderer, playerTexture);
                SDL_SetRenderDrawColor(renderer, 255, 255, 255, 255);
                SDL_RenderClear(renderer);

                //Reseting the rendere trarget to the screen
                SDL_SetRenderTarget(renderer, IntPtr.Zero);

                //event listener

                while (SDL_PollEvent(out events) == 1)
                {
                    switch (events.type)
                    {
                        case SDL.SDL_EventType.SDL_QUIT:
                            pause = true;
                            while (pause) {

                            }
                            loop = !true;
                            break;
                        default:
                            break;
                    }
                    //key 
                    if (events.type == SDL_EventType.SDL_KEYDOWN) {
                        if (GetKey(SDL_Keycode.SDLK_RIGHT) == true) {
                            x_pos += 100 * (TARGET_FPS / 1000);
                            Console.WriteLine(x_pos);
                            rect.x += (int)x_pos;
                            posPlayer.RELATIVE_POS_X += 10;
                        }
                        if (GetKey(SDL_Keycode.SDLK_LEFT) == true)
                        {
                            x_pos -= 100 * (deltaTime / 1000);
                            rect.x -= (int)x_pos;
                            posPlayer.RELATIVE_POS_X -= 10;
                        }
                        if (GetKey(SDL_Keycode.SDLK_UP) == true)
                        {
                            y_pos -= 100 * (deltaTime / 1000);
                            rect.y -= (int)y_pos;
                            posPlayer.RELATIVE_POS_Y -= 10;
                        }
                        if (GetKey(SDL_Keycode.SDLK_DOWN) == true)
                        {
                            y_pos += 100 * (deltaTime / 1000);
                            rect.y += (int)y_pos;
                            posPlayer.RELATIVE_POS_Y += 10;
                        }
                        if (GetKey(SDL_Keycode.SDLK_ESCAPE) == true)
                        {
                            loop = !true;
                        }
                        if (GetKey(SDL_Keycode.SDLK_SPACE))
                        {
                            while (rectBullet.x < Area.w && rectBullet.y < Area.h) {
                                x_posB += 10 * (TARGET_FPS / 1000);
                                rectBullet.x = (int)x_posB;
                                Console.WriteLine((int)BullertBuffer.x_pos);
                                oldTime = newTime;
                                SDL_RenderCopy(renderer, playerTexture, IntPtr.Zero, ref rect);
                                SDL_RenderCopy(renderer, bulletTexture, ref Area, ref rectBullet);
                                SDL_RenderPresent(renderer);
                            }
                            BullertBuffer.x_pos = 0;
                            rectBullet.x = posPlayer.RELATIVE_POS_X;
                            rectBullet.y = posPlayer.RELATIVE_POS_Y;
                        }
                    }

                }

                //Giving the rendrer a texture to rendreer with a dsrect/object.
                oldTime = newTime;
                SDL_RenderCopy(renderer, playerTexture, IntPtr.Zero, ref rect);
                SDL_RenderPresent(renderer);
            }



            SDL_DestroyTexture(playerTexture);
            SDL_DestroyWindow(window);
            SDL_DestroyRenderer(renderer);
            return 0;
        }
        static bool GetKey(SDL.SDL_Keycode _keycode)
        {
            int arraySize;
            bool isKeyPressed = false;
            IntPtr origArray = SDL.SDL_GetKeyboardState(out arraySize);
            byte[] keys = new byte[arraySize];
            byte keycode = (byte)SDL.SDL_GetScancodeFromKey(_keycode);
            Marshal.Copy(origArray, keys, 0, arraySize);
            isKeyPressed = keys[keycode] == 1;
            return isKeyPressed;
        }

        
    }
    
}



