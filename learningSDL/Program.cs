using System;
using System.Runtime.InteropServices;
using System.Xml.Schema;
using SDL2;
using static SDL2.SDL;

namespace TesingAndLearning
{
    class Program
    {

        static int Main(String[] args) {

            var window = IntPtr.Zero;
            var renderer = IntPtr.Zero;

            SDL_Init(SDL_INIT_VIDEO);

            SDL_CreateWindowAndRenderer(800, 600,0,out window,out renderer);
            SDL_SetWindowTitle(window, "SDL LEARNING");
            SDL_SetRenderDrawColor(renderer, 255, 0,0,255);
            SDL_RenderClear(renderer);

            SDL_Rect rect;
            rect.h = 100;
            rect.w = 100;
            rect.x = 5;
            rect.y = 5;

            SDL_Rect rect1;
            rect1.h = 100;
            rect1.w = 100;
            rect1.x = 50;
            rect1.y = 50;

            //SDL_Rect intersection;

            //SDL_IntersectRect(ref rect, ref rect1, out intersection);
            //SDL_SetRenderDrawColor(renderer, 0,0,255,255);
            //SDL_RenderFillRect(renderer, ref intersection);
            SDL_Event events;
            IntPtr keyboardState;
            int arrayKeys;
            //Main loop
            bool loop = false;
            while (!loop)
            {
                keyboardState = SDL_GetKeyboardState(out arrayKeys);
                //Console.WriteLine(arrayKeys);
                //event listener
                while (SDL_PollEvent(out events) == 1)
                {
                    switch (events.type)
                    {
                        case SDL.SDL_EventType.SDL_QUIT:
                            loop = true;
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
                            loop = true;
                        }
                    }

                }

                SDL_SetRenderDrawColor(renderer, 255, 0, 0, 255);
                SDL_RenderClear(renderer);

                SDL_SetRenderDrawColor(renderer, 255, 255, 255, 255);
                SDL_RenderDrawPoint(renderer, 800 / 2, 600 / 2);

                SDL_SetRenderDrawColor(renderer, 255, 255, 255, 255);

                SDL_RenderDrawRect(renderer, ref rect);
                SDL_SetRenderDrawColor(renderer, 9, 255, 9, 255);
                SDL_RenderDrawRect(renderer, ref rect1);

                SDL_RenderPresent(renderer);
            }

            

            

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
        void notUSED()
        {
            if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
            {
                Console.WriteLine("ERROR INNIT");
            }

            int SCREEN_WIDTH = 1000;
            int SCREEN_HEIGHT = 1000;

            // int OBJECT_WIDTH = 100;
            // int OBJECT_HEIGHT = 100;


            // int startPosX = (SCREEN_WIDTH / 2) - (OBJECT_WIDTH  / 2);
            // int startPosY = (SCREEN_HEIGHT / 2) - (OBJECT_HEIGHT / 2);

            // int RelativePosX = startPosX;
            // int RelativePosY = startPosY;

            Console.WriteLine("SDL init succeede");
            bool loop = false;
            var window = IntPtr.Zero;
            var renderer = IntPtr.Zero;
            var texture = IntPtr.Zero;
            var red_texture = IntPtr.Zero;
            var blue_texture = IntPtr.Zero;


            // Init Window
            window = SDL_CreateWindow("Hello World", SDL_WINDOWPOS_CENTERED,
                SDL_WINDOWPOS_CENTERED, SCREEN_WIDTH, SCREEN_HEIGHT, SDL_WindowFlags.SDL_WINDOW_SHOWN);

            //Init Renderer
            renderer = SDL_CreateRenderer(window, -1, SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

            //Init texture for image
            texture = SDL_CreateTextureFromSurface(renderer, SDL_LoadBMP("image.bmp"));

            //Init texture/Creating a texture
            blue_texture = SDL_CreateTexture(renderer, SDL_PIXELFORMAT_RGBA8888,
                (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET, 100, 100);

            //init
            SDL_Rect dstrect;
            //Object position/size
            dstrect.x = 20;
            dstrect.y = 20;
            dstrect.h = 100;
            dstrect.w = 100;

            //Which texture should the renderer render/target
            SDL_SetRenderTarget(renderer, blue_texture);
            //Which color should the renderer draw on the texture
            SDL_SetRenderDrawColor(renderer, 0, 255, 0, 255);
            //Clears the rendrere for the new rendrer aka so that the texture would appear and not black
            SDL_RenderClear(renderer);

            //Setting the rendere back to the window aka not pointing it to junk memory (which is bad)
            SDL_SetRenderTarget(renderer, IntPtr.Zero);

            //Taking a copy of our now drawn texture to the rendrer
            SDL_RenderCopy(renderer, blue_texture, IntPtr.Zero, ref dstrect);
            //Displaying the rendrers to the screen
            SDL_RenderPresent(renderer);

            //Init event
            SDL_Event events;
            //Main loop
            while (!loop)
            {

                //event listener
                while (SDL_PollEvent(out events) == 1)
                {
                    switch (events.type)
                    {
                        case SDL.SDL_EventType.SDL_QUIT:
                            loop = true;
                            break;
                        default:
                            break;
                        case SDL.SDL_EventType.SDL_KEYDOWN:
                            //key listener
                            switch (events.key.keysym.sym)
                            {
                                case SDL.SDL_Keycode.SDLK_ESCAPE:
                                    loop = true;
                                    break;
                                case SDL_Keycode.SDLK_RIGHT:
                                    dstrect.x += 10;
                                    var pixles = IntPtr.Size;
                                    SDL_UpdateTexture(blue_texture, ref dstrect, IntPtr.MaxValue, 100 * 4);
                                    Console.WriteLine(dstrect.x);

                                    break;

                            }
                            break;
                    }
                }

                //Taking a copy of our now drawn texture to the rendrer
                //Displaying the rendrers to the screen
                SDL_RenderPresent(renderer);

            }

            //When the program is finished
            SDL.SDL_DestroyTexture(blue_texture);
            SDL.SDL_DestroyRenderer(renderer);
            SDL.SDL_DestroyWindow(window);
            SDL.SDL_Quit();

        }
    }
    
}



