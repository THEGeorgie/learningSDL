using System;
using System.Runtime.InteropServices;
using System.Xml.Schema;
using SDL2;
using static SDL2.SDL;

namespace TesingAndLearning
{
    class Program
    {

        struct Clock
        {
            public Clock()
            {
            }
            UInt32 last_tick_time = 0;
            public int delta = 0;
            void tick()
            {
                UInt32 tick_time = SDL_GetTicks();
                delta = tick_time - last_tick_time;
                last_tick_time = tick_time;
            }
        }

        static int Main(String[] args) {



            var window = IntPtr.Zero;
            var renderer = IntPtr.Zero;
            var playerTexture = IntPtr.Zero;
            var bulletTexture = IntPtr.Zero;
            int SCREEN_WIDTH = 800;
            int SCREEN_HEIGHT = 600;

            SDL_Init(SDL_INIT_VIDEO);

            SDL_CreateWindowAndRenderer(SCREEN_WIDTH, SCREEN_HEIGHT, 0,out window,out renderer);

            playerTexture = SDL_CreateTexture(renderer, SDL_PIXELFORMAT_RGBA8888,
                (int)SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET, 100, 100);
            bulletTexture = SDL_CreateTextureFromSurface(renderer, SDL_image.IMG_Load("bullet.png"));

            SDL_SetWindowTitle(window, "SDL LEARNING");
            SDL_SetRenderDrawColor(renderer, 255, 0,0,255);
            SDL_RenderClear(renderer);

            SDL_Rect rect;
            rect.h = 100;
            rect.w = 100;
            rect.x = 5;
            rect.y = 5;
            SDL_Rect rectBullet;
            rectBullet.h = 20;
            rectBullet.w = 20;
            rectBullet.x = 5;
            rectBullet.y = 5;
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
            while (loop)
            {
                keyboardState = SDL_GetKeyboardState(out arrayKeys);
                Clock clock;


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
                            loop = !true;
                            break;
                        default:
                            break;
                    }
                    //key listener
                    if (events.type == SDL_EventType.SDL_KEYDOWN) {
                        if (GetKey(SDL_Keycode.SDLK_RIGHT) == true) {
                            rect.x += 10;
                        }
                        if (GetKey(SDL_Keycode.SDLK_LEFT) == true)
                        {
                            rect.x -= 10;
                        }
                        if (GetKey(SDL_Keycode.SDLK_UP) == true)
                        {
                            rect.y -= 10;
                        }
                        if (GetKey(SDL_Keycode.SDLK_DOWN) == true)
                        {
                            rect.y += 10;
                        }
                        if (GetKey(SDL_Keycode.SDLK_ESCAPE) == true)
                        {
                            loop = !true;
                        }
                        if (GetKey(SDL_Keycode.SDLK_SPACE))
                        {
                            while (rectBullet.x < Area.w && rectBullet.y < Area.h) {
                                rectBullet.x += 1 * clock.delta;
                                SDL_RenderCopy(renderer, playerTexture, IntPtr.Zero, ref rect);
                                SDL_RenderCopy(renderer, bulletTexture, ref Area, ref rectBullet);
                                SDL_RenderPresent(renderer);
                            }
                            rectBullet.x = 0;
                            rectBullet.y = 0;
                        }
                    }

                }

                //Giving the rendrer a texture to rendreer with a dsrect/object.
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



