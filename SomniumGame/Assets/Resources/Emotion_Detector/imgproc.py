from cv2 import imread
from cv2 import cvtColor
from cv2 import COLOR_BGR2GRAY
from cv2 import CascadeClassifier
from cv2 import resize
from cv2 import imwrite

#from mtcnn.mtcnn import MTCNN


class EmotionDetection():
    ''' Find faces in an image and crop them out. Then resize and save them. '''
    def __init__(self, app_path, face_size=64):
        self.app_path = app_path
        self.face_size = face_size

    
    def run(self):
        '''
        Crop faces out of the frame and make a prediction
        on what emotion they're experiencing.
        '''
        # Fetch the frame and convert it to grayscale
        frame = imread(self.app_path + r"/Resources/Model_Images/Cur_Frame/frame.jpg")
        frame_gray = cvtColor(frame, COLOR_BGR2GRAY)

        # Get the height and width
        self.height, self.width = frame_gray.shape

        # Detect all faces in the frame, extract them, and save them for
        # the C# script to make use of.
        faces_data = self.detect_faces_haar(frame)
        faces = self.extract_faces(frame_gray, faces_data)
        self.save_faces(faces)


    #def detect_faces_MTCNN(self, frame):
        #''' Find the faces using the MTCNN model. '''
        #detector = MTCNN()
        #return detector.detect_faces(frame)


    def detect_faces_haar(self, frame_gray):
        ''' Use Haar cascades to detect all faces in the frame. '''
        # Load the cascade
        #path = self.app_path + r"/Resources/Haar_Cascades/haarcascade_frontalface_alt.xml"
        #path = self.app_path + r"/Resources/Haar_Cascades/haarcascade_frontalface_alt2.xml"
        path = self.app_path + r"/Resources/Haar_Cascades/haarcascade_frontalface_default.xml"
        cascade = CascadeClassifier(path)

        # Detect faces
        return cascade.detectMultiScale(image=frame_gray, scaleFactor=1.15, minNeighbors=5)


    def extract_faces(self, frame_gray, faces_data):
        '''
        Crop the faces out of the frame, resize them, and
        convert them into arrays.
        '''
        faces = []
        for face_data in faces_data:
            # Get the min/max coords
            #xmin, ymin, width, height = face_data["box"] # For using MTCNN
            xmin, ymin, width, height = face_data # For using a Haar cascade
            xmax, ymax = xmin + width, ymin + height
            
            # Crop the face
            face = frame_gray[ymin:ymax, xmin:xmax]

            # Resize and convert to an array, then add to the list
            faces.append(resize(face, (self.face_size, self.face_size)))
        return faces
    
    
    def save_faces(self, faces):
        ''' Save the faces as images to be loaded in the C# script. '''
        for i, face in enumerate(faces):
            path = self.app_path + r"\Resources\Model_Images\Cur_Faces\face{}.jpg".format(i)
            imwrite(path, face)


if __name__ == "__main__":
    ed = EmotionDetection(r"C:/Users/Grant/Documents/GitHub/Somnium/SomniumGame/Assets")
    ed.run()
