# CS X-ray Recovery Tool - C# Edition
This repository contains the code for the Carestream X-ray Recovery Tool.

## Introduction

The CS X-ray Recovery Tool, now rewritten in C# with a modular class structure, provides an advanced solution for retrieving lost X-ray images from the Carestream CS7600 system and saving them to the user's desktop. This version enhances functionality with a robust graphical interface that previews images and simplifies user interaction, tailored specifically for dental clinics and medical imaging environments requiring a dependable backup or recovery of patient X-ray images.

## Overview

The C# version introduces a structured approach by separating functionalities into distinct classes:

1. **LostXray Finder**: Scans the specified directories for unimported X-ray images due to system crashes or network issues.
2. **Recovery Service**: Handles the retrieval and storage of the found X-ray images in a structured directory on the desktop.
3. **Error Handling**: Implements custom exception handling to ensure the tool's stability and provide detailed error logs.
4. **User Interface**: Provides a Windows Forms-based interface that lists all discovered X-rays with an image preview feature, enhancing the user's ability to verify and recover images.
5. **Packaging**: The tool is packaged as an executable using Costura.Fody, streamlining installation and distribution without dependency concerns.

## Detailed Workflow

1. **LostXray Finder**
   - The `CS7600XrayFinder` class identifies lost images by scanning designated directories for X-rays that were not imported into the patient's file.

2. **Recovery Service**
   - The `RecoveryService` class ensures the safe retrieval and saving of lost X-ray images, organizing them into a user-friendly directory system.

3. **Error Handling**
   - Errors are captured and managed throughout the tool, preventing system crashes and allowing for comprehensive troubleshooting.

4. **User Interface**
   - The intuitive UI allows users to select and preview X-rays before recovery, making it accessible to non-technical staff.

5. **Packaging**
   - The executable format ensures ease of use across various systems, promoting a plug-and-play experience for the user.

## Usage

1. Execute the tool with the necessary permissions.
2. The UI displays a list of found X-rays along with thumbnail previews.
3. Select an X-ray and use the "Recover" button to save images to a "RecoveredXrays" folder on the desktop.
4. Follow on-screen prompts and messages for successful recovery or troubleshooting steps.

## Conclusion

The C# edition of the CS X-ray Recovery Tool is a significant leap forward in user experience and reliability, offering a sophisticated yet user-friendly interface for the recovery of patient X-ray images. This tool is a vital asset for healthcare facilities relying on the Carestream CS7600 system, ensuring the preservation and accessibility of crucial medical images.
