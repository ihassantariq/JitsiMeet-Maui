from PIL import Image

def pad_image(path, target_size, bg_color):
    try:
        img = Image.open(path)
        # Create a new image with target size and background color
        new_img = Image.new("RGBA", target_size, bg_color)
        
        # Calculate position to center the original image
        paste_x = (target_size[0] - img.width) // 2
        paste_y = (target_size[1] - img.height) // 2
        
        # Paste original image onto the new one
        if img.mode == 'RGBA':
            new_img.paste(img, (paste_x, paste_y), img)
        else:
            new_img.paste(img, (paste_x, paste_y))
            
        new_img.save(path)
        print(f"Padded {path} to {target_size}")
    except Exception as e:
        print(f"Error padding {path}: {e}")

# Apply padding (Transparent for App Icon foreground, #121212 for Splash Screen)
pad_image("JitsiMeetDemo/Resources/AppIcon/appiconfg.png", (256, 256), (0, 0, 0, 0))
pad_image("JitsiMeetDemo/Resources/Splash/splash.png", (256, 256), (18, 18, 18, 255)) # #121212 is rgb(18,18,18)
