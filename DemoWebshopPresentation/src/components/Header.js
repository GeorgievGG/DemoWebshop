import { useLocation } from "react-router-dom";
import PropTypes from 'prop-types'
import Button from './Button'

const Header = (props) => {
    const location = useLocation()

    return (
        <header>
            <h1>{props.title}</h1>
            {
                location.pathname === '/'  &&
                <Button color={props.showAdd ? 'orange' : 'lightgreen' } text={props.showAdd ? 'Close' : 'Add' } onClick={props.onAdd} />
            }
        </header>
    )
}

Header.defaultProps = {
    title: 'Task Tracker'
}

Header.propTypes = {
    title: PropTypes.string,
    showAdd: PropTypes.bool,
    onAdd: PropTypes.func
}

export default Header