import { useRef } from "react";
import ClearButton from "../buttons/ClearButton/ClearButton";
import styles from "./ImagesSlider.module.css";

const ImagesSlider = ({images = []}) => {
  const imageContainer = useRef(null);
  
  const handleScroll = (direction = "forward") => {
    if(imageContainer.current != null && images.length > 1)
    {
        const curentPos = imageContainer.current.scrollLeft;
        const maxScroll = imageContainer.current.scrollWidth - imageContainer.current.clientWidth;
        const difScroll = maxScroll / (images.length - 1);
        imageContainer.current.scrollTo({
          left: direction === "forward" ? 
            curentPos + difScroll :
            curentPos - difScroll,
          behavior: "smooth"
        });
    }
  }

  return(
    <div className={styles.ImagesSlider__Main}>
      <div ref={imageContainer} 
      className={styles.ImagesSlider__Images}>
        {
          images.map((image, index) => (
          <div key={index}
            className={styles.ImagesSlider__SelectedImage}
            style={{backgroundImage: `url(${image})`}}>

          </div>))
        }
      </div>
      <div className={styles.ImagesSlider__Btn}>
        <ClearButton action={() => handleScroll("backward")}>
          <svg fill="#222" height="15px" width="15px" version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlnsXlink="http://www.w3.org/1999/xlink" viewBox="0 0 330 330" xmlSpace="preserve" stroke="#000" transform="matrix(-1, 0, 0, 1, 0, 0)"><g id="SVGRepo_bgCarrier" strokeWidth="1"></g><g id="SVGRepo_tracerCarrier" strokeLinecap="roubd" strokeLinejoin="round"></g><g id="SVGRepo_iconCarrier"> <path id="XMLID_222_" d="M250.606,154.389l-150-149.996c-5.857-5.858-15.355-5.858-21.213,0.001 c-5.857,5.858-5.857,15.355,0.001,21.213l139.393,139.39L79.393,304.394c-5.857,5.858-5.857,15.355,0.001,21.213 C82.322,328.536,86.161,330,90,330s7.678-1.464,10.607-4.394l149.999-150.004c2.814-2.813,4.394-6.628,4.394-10.606 C255,161.018,253.42,157.202,250.606,154.389z"></path> </g></svg>
        </ClearButton>
      </div>
      <div className={`${styles.ImagesSlider__Btn} ${styles.ImagesSlider__BtnRight}`}>
        <ClearButton action={() => handleScroll("forward")}>
          <svg fill="#222" height="15px" width="15px" version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlnsXlink="http://www.w3.org/1999/xlink" viewBox="0 0 330 330" xmlSpace="preserve" stroke="#fff"><g id="SVGRepo_bgCarrier" strokeWidth="1"></g><g id="SVGRepo_tracerCarrier" strokeLinecap="roubd" strokeLinejoin="round"></g><g id="SVGRepo_iconCarrier"> <path id="XMLID_222_" d="M250.606,154.389l-150-149.996c-5.857-5.858-15.355-5.858-21.213,0.001 c-5.857,5.858-5.857,15.355,0.001,21.213l139.393,139.39L79.393,304.394c-5.857,5.858-5.857,15.355,0.001,21.213 C82.322,328.536,86.161,330,90,330s7.678-1.464,10.607-4.394l149.999-150.004c2.814-2.813,4.394-6.628,4.394-10.606 C255,161.018,253.42,157.202,250.606,154.389z"></path> </g></svg>
        </ClearButton>
      </div>
    </div>
  );
}

export default ImagesSlider;