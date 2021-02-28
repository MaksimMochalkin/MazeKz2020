const $ = {}

Element.prototype.appendAfter = function(element) {
    element.parentNode.insertBefore(this, element.nextSibling);

}

function _createModalFooter(buttons = []) {
    if(buttons.length === 0) {
        return document.createElement('div')
    }

    const wrap = document.createElement('div')
    wrap.classList.add('modal__footer')

    buttons.forEach(btn => {
        const $btn = document.createElement('button')
        $btn.textContent = btn.text
        $btn.classList.add('btn')
        $btn.classList.add(`btn-${btn.type || 'secondary'}`)
        $btn.onclick = btn.handler || noop

        wrap.appendChild($btn)
    })

    return wrap
}

function _createModal(options) {
    const DEFAULT_WIDTH = '600px'
    const modal = document.createElement('div')
    modal.classList.add('dmodal')
    modal.insertAdjacentHTML('afterbegin', `
        <div class="modal__overlay" data-close="true">
            <div class="modal__window" style="width: ${options.width || DEFAULT_WIDTH}">
                <div class="modal__header">
                    <span class="modal__title">${options.title || ''}</span>
                    ${options.closabel ? `<span class="modal__close" data-close="true">&times;</span>` : ''}
                </div>
                <div class="modal__body" data-content>
                    ${options.content || ''}
                </div>
            </div>
        </div>
    `)

    const footer = _createModalFooter(options.footerButtons)
    footer.appendAfter(modal.querySelector('[data-content]'))
    document.body.appendChild(modal)
    return modal
}

$.modal = function(options) {
    const ANIMATION_SPEED = 200
    const $modal = _createModal(options)
    let closing = false
    let destroyed = false

    const modal = {
        open() {
            if(destroyed) {
                return console.log('Modaal is destroyed')
            }
            !closing && $modal.classList.add('open')
        },
        close() {
            closing = true
            $modal.classList.remove('open')
            $modal.classList.add('hidden')
            setTimeout(() => {
                $modal.classList.remove('hidden')
                closing = false
                if(typeof options.onClose === 'function') {
                    options.onClose()
                }
            }, ANIMATION_SPEED)
        }
        
    }

    const listener = event => {
        if(event.target.dataset.close) {
            modal.close()
        }
    }

    $modal.addEventListener('click', listener)

    return Object.assign(modal, {
        destroy() {
            $modal.parentNode.removeChild($modal)
            $modal.removeEventListener('click', listener)
            destroyed = true
        },
        setContent(html) {
            $modal.querySelector('[data-content]').innerHTML = html
        }
    })
}

let fruits = [
    {id: 1, title: 'Ананас', price: 20, img: 'https://cs11.pikabu.ru/post_img/big/2019/02/18/10/1550507035113884543.png'},
    {id: 2, title: 'Яблоко', price: 30, img: 'https://st.depositphotos.com/1003272/1632/i/600/depositphotos_16322913-stock-photo-red-apple.jpg'},
    {id: 3, title: 'Арбуз', price: 40, img: 'https://n1s1.hsmedia.ru/d5/ce/d0/d5ced00723c28f9070fecd1d78c080db/620x462_1_71b4be926abea22bb11dd852cf6a4ff5@1000x745_0xac120003_2747968621562649731.jpg'}
]

const toHtml = fruits => `
            <div class="col">
                <div class="card">
                    <img style="height: 300px;" src=${fruits.img} class="card-img-top" alt=${fruits.title}>
                    <div class="card-body">
                      <h5 class="card-title">${fruits.title}</h5>
                      <a href="#" class="btn btn-primary" data-btn="price" data-id=${fruits.id}>Посмотреть цену</a>
                      <a href="#" class="btn btn-danger" data-btn="remove" data-id=${fruits.id}>Удалить</a>
                    </div>
                  </div>
            </div>
`

function render() {
    const html = fruits.map(fruit => toHtml(fruit)).join('')
    document.querySelector('#fruits').innerHTML = html
}

render()

const priceModal = $.modal({
    title: 'Цена на фрукт',
    closabel: true,
    width: '400px',
    footerButtons: [
        {text: 'OK', type: 'primary', handler() {
            priceModal.close()
        }},
        {text: 'Отменить', type: 'danger', handler() {
            priceModal.close()
        }}
    ]
})

document.addEventListener('click', event =>{
    event.preventDefault()
    const btnType = event.target.dataset.btn
    const id = +event.target.dataset.id
    const fruit = fruits.find(x => x.id === id)

    if(btnType === 'price') {
        priceModal.setContent(`
            <p>Цена на ${fruit.title}: <strong>${fruit.price}$</strong></p>

        `)
        priceModal.open()
    } else if(btnType === 'remove') {
        $.confirm( {
            title: 'Вы уверены?',
            content: `<p>Удалить: <strong>${fruit.title}</strong></p>`
        }).then(() => {
            fruits = fruits.filter(f => f.id !== id)
            render()
        }).catch(() => {
            console.log('cancel')
        })
    } 
})

$.confirm = function(options) {
    return new Promise((resolve, reject) => {
        const modal = $.modal({
            title: options.title,
            width: '400px',
            closable: false,
            content: options.content,
            onClose() {
                modal.destroy()
            },
            footerButtons: [
                {text: 'Отменить', type: 'secondary', handler() {
                    modal.close()
                    reject()
                }},
                {text: 'Удалить', type: 'danger', handler() {
                    modal.close()
                    resolve()
                }}
            ]
        })
        setTimeout( () => modal.open(), 100)
    })
}